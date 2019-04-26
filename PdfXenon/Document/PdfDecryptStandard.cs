using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace PdfXenon.Standard
{
    public class PdfDecryptStandard : PdfDecrypt
    {
        private static byte[] PADDING_32 = { 0x28, 0xBF, 0x4E, 0x5E, 0x4E, 0x75, 0x8A, 0x41, 0x64, 0x00, 0x4E, 0x56, 0xFF, 0xFA, 0x01, 0x08,
                                             0x2E, 0x2E, 0x00, 0xB6, 0xD0, 0x68, 0x3E, 0x80, 0x2F, 0x0C, 0xA9, 0xFE, 0x64, 0x53, 0x69, 0x7A };

        private MD5 _md5 = MD5.Create();
        private byte[] _encryptionKey;

        public PdfDecryptStandard(PdfObject parent, PdfDictionary trailer, PdfDictionary encrypt)
            : base(parent)
        {
            // Extract the first document identifier from the trailer
            PdfArray ids = trailer.MandatoryValue<PdfArray>("ID");
            PdfString id0 = (PdfString)ids.Objects[0];

            // Extract and check the mandatory fields
            PdfInteger R = encrypt.MandatoryValue<PdfInteger>("R");
            PdfInteger keyLength = encrypt.MandatoryValue<PdfInteger>("Length");
            PdfString O = encrypt.MandatoryValue<PdfString>("O");
            PdfString U = encrypt.MandatoryValue<PdfString>("U");
            PdfInteger P = encrypt.MandatoryValue<PdfInteger>("P");

            if (R.Value != 3)
                throw new ApplicationException("Cannot decrypt standard handler with revision other than 3.");

            if ((keyLength.Value < 40) || (keyLength.Value > 128) || (keyLength.Value % 8 != 0))
                throw new ApplicationException("Cannot decrypt with key length < 40 or > 128 or not a multiple of 8.");

            // Setup by owner password
            byte[] ownerPasswordValue = ComputeOwnerPasswordValue(keyLength.Value, O.ValueAsBytes);
            _encryptionKey = ComputeEncryptionKey(keyLength.Value, id0.ValueAsBytes, ownerPasswordValue, O.ValueAsBytes, P.Value);
            byte[] userPasswordValue = ComputeUserPasswordValue(keyLength.Value, id0.ValueAsBytes, _encryptionKey);

            // If the owner password does not match...
            byte[] UBytes = U.ValueAsBytes;
            if (!CompareArray(userPasswordValue, UBytes, 16))
            {
                // ...then try and use the user password instead...
                _encryptionKey = ComputeEncryptionKey(keyLength.Value, id0.ValueAsBytes, PADDING_32, O.ValueAsBytes, P.Value);
                userPasswordValue = ComputeUserPasswordValue(keyLength.Value, id0.ValueAsBytes, _encryptionKey);

                // If the user password does not match either..
                if (!CompareArray(userPasswordValue, UBytes, 16))
                    throw new ApplicationException("Cannot decrypt using a blank owner or user password.");
            }
        }

        public override string DecodeString(PdfString obj)
        {
            return obj.ParseString.BytesToString(DecodeBytes(obj, obj.ParseString.ValueAsBytes));
        }

        public override byte[] DecodeStringAsBytes(PdfString obj)
        {
            return DecodeBytes(obj, obj.ParseString.ValueAsBytes);
        }

        public override string DecodeStream(PdfStream stream)
        {
            return Encoding.ASCII.GetString(stream.ParseStream.DecodeBytes(DecodeBytes(stream, stream.ParseStream.StreamBytes)));
        }

        public override byte[] DecodeStreamAsBytes(PdfStream stream)
        {
            return stream.ParseStream.DecodeBytes(DecodeBytes(stream, stream.ParseStream.StreamBytes));
        }

        private byte[] DecodeBytes(PdfObject obj, byte[] bytes)
        {
            PdfIndirectObject indirectObject = obj.TypedParent<PdfIndirectObject>();
            if (indirectObject == null)
                throw new ApplicationException($"Cannot decrypt a string that is not inside an indirect object.");

            // Create bytes that need hashing by combining the encryption key with the indirect object numbers
            byte[] key = new byte[_encryptionKey.Length + 5];
            Array.Copy(_encryptionKey, 0, key, 0, _encryptionKey.Length);
            int index = _encryptionKey.Length;
            int id = indirectObject.Id;
            key[index++] = (byte)(id >> 0);
            key[index++] = (byte)(id >> 8);
            key[index++] = (byte)(id >> 16);
            int gen = indirectObject.Gen;
            key[index++] = (byte)(gen >> 0);
            key[index++] = (byte)(gen >> 8);

            // MD5 hash the bytes to get raw decrypt key
            key = _md5.ComputeHash(key);

            // Limit check the decrypt key length
            int keyLength = _encryptionKey.Length + 5;
            if (keyLength > 16)
                keyLength = 16;

            // Create the RC4 key
            byte[] rc4Key = new Byte[keyLength];
            Array.Copy(key, 0, rc4Key, 0, keyLength);
            return RC4.Transform(rc4Key, bytes);
        }

        private byte[] ComputeEncryptionKey(int keyLength, byte[] documentId, byte[] ownerPasswordValue, byte[] ownerKey, int permissions)
        {
            // Algorithm 3.2, Computing an encryption key

            // (1, 2, 3, 4, 5) Appends all required data that needs to be MD5 hashed
            byte[] hash = new byte[ownerPasswordValue.Length + ownerKey.Length + 4 + documentId.Length];
            Array.Copy(ownerPasswordValue, 0, hash, 0, ownerPasswordValue.Length);
            Array.Copy(ownerKey, 0, hash, ownerPasswordValue.Length, ownerKey.Length);
            hash[ownerPasswordValue.Length + ownerKey.Length + 0] = (byte)(permissions >> 0);
            hash[ownerPasswordValue.Length + ownerKey.Length + 1] = (byte)(permissions >> 8);
            hash[ownerPasswordValue.Length + ownerKey.Length + 2] = (byte)(permissions >> 16);
            hash[ownerPasswordValue.Length + ownerKey.Length + 3] = (byte)(permissions >> 24);
            Array.Copy(documentId, 0, hash, ownerPasswordValue.Length + ownerKey.Length + 4, documentId.Length);

            // (7) Hash using MD5
            hash = _md5.ComputeHash(hash);

            // (8) Rehash 50 times
            int blockLength = keyLength / 8;
            byte[] block = new byte[blockLength];
            Array.Copy(hash, 0, block, 0, block.Length);
            for (int i = 0; i < 50; i++)
                Array.Copy(_md5.ComputeHash(block), 0, block, 0, block.Length);

            // (9) Return only the keyLength related number of bytes
            return block;
        }

        private byte[] ComputeOwnerPasswordValue(int keyLength, byte[] ownerKey)
        {
            // Algorithm 3.3, Computing the encryption dictionary's O (owner password) value

            // (1) Pad the owner password (use the pad because we do not care about a users password)
            byte[] ownerPasword = new byte[32];
            Array.Copy(PADDING_32, ownerPasword, 32);

            // (2) Initialize the MD5
            ownerPasword = _md5.ComputeHash(ownerPasword);

            // (3) Rehash 50 times
            int blockLength = keyLength / 8;
            byte[] block = new byte[blockLength];
            Array.Copy(ownerPasword, 0, block, 0, block.Length);
            for (int i = 0; i < 50; i++)
                Array.Copy(_md5.ComputeHash(block), 0, block, 0, block.Length);

            // (4) RC4 key
            byte[] rc4Key = new byte[blockLength];

            // (5) Pad the user password
            byte[] userPassword = new byte[32];
            Array.Copy(PADDING_32, userPassword, 32);

            // (6, 7) Iterate 20 times
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < blockLength; ++j)
                    rc4Key[j] = (byte)(block[j] ^ i);

                ownerKey = RC4.Transform(rc4Key, ownerKey);
            }

            return ownerKey;
        }

        private byte[] ComputeUserPasswordValue(int keyLength, byte[] documentId, byte[] encryptionKey)
        {
            // Algorithm 3.5, Computing the encryption dictionary's U (user password) value

            // (2, 3) Join the fixed padding and the document identifier
            byte[] hash = new byte[PADDING_32.Length + documentId.Length];
            Array.Copy(PADDING_32, 0, hash, 0, PADDING_32.Length);
            Array.Copy(documentId, 0, hash, PADDING_32.Length, documentId.Length);

            // (3) Hash using MD5
            hash = _md5.ComputeHash(hash);

            // (4, 5) Iterate 20 times
            byte[] rc4Key = new byte[encryptionKey.Length];
            for (int i = 0; i < 20; i++)
            {
                for (int j = 0; j < encryptionKey.Length; ++j)
                    rc4Key[j] = (byte)(encryptionKey[j] ^ i);

                hash = RC4.Transform(rc4Key, hash);
            }

            // (6) First 16 bytes is the hash result and the remainder is zero's
            byte[] userKey = new byte[32];
            Array.Copy(hash, 0, userKey, 0, 16);
            return userKey;
        }

        private bool CompareArray(byte[] l, byte[] r, int length)
        {
            for (int i = 0; i < length; i++)
                if (l[i] != r[i])
                    return false;

            return true;
        }
    }
}
