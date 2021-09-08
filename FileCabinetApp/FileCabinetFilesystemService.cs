﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;

namespace FileCabinetApp
{
    public class FileCabinetFilesystemService : IFileCabinetService
    {
        private FileStream fileStream;
        private static readonly int maxStringLength = 120;
        private static readonly int recordSize = sizeof(short) * 2 + sizeof(int) * 4 + maxStringLength * 2 + sizeof(char) + sizeof(decimal);

        public FileCabinetFilesystemService(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        public int CreateRecord(RecordParameters recordParameters)
        {
            int id = (int)(fileStream.Length == 0 ? 1 : (fileStream.Length / recordSize) + 1);
            fileStream.Seek(0, SeekOrigin.End);
            BinaryWriter writer = new BinaryWriter(fileStream);

            writer.Write(new byte[2]);
            writer.Write(id);

            char[] firstNameChars = new char[maxStringLength];
            char[] lastNameChars = new char[maxStringLength];

            Array.Copy(recordParameters.FirstName.ToCharArray(), firstNameChars, recordParameters.FirstName.Length);
            Array.Copy(recordParameters.LastName.ToCharArray(), lastNameChars, recordParameters.LastName.Length);

            writer.Write(Encoding.Unicode.GetBytes(firstNameChars), 0, maxStringLength);
            writer.Write(Encoding.Unicode.GetBytes(lastNameChars), 0, maxStringLength);

            writer.Write(recordParameters.DateOfBirth.Year);
            writer.Write(recordParameters.DateOfBirth.Month);
            writer.Write(recordParameters.DateOfBirth.Day);
            writer.Write(Encoding.Unicode.GetBytes(new char[] { recordParameters.Gender }));
            writer.Write(recordParameters.Experience);
            writer.Write(recordParameters.Salary);

            writer.Close();

            return id;
        }

        public void EditRecord(int id, RecordParameters recordParameters)
        {
            fileStream.Seek(2, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(fileStream);
            BinaryWriter writer = new BinaryWriter(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                int currentRecordId = reader.ReadInt32();
                if (currentRecordId == id)
                {
                    char[] firstNameChars = new char[maxStringLength];
                    char[] lastNameChars = new char[maxStringLength];

                    Array.Copy(recordParameters.FirstName.ToCharArray(), firstNameChars, recordParameters.FirstName.Length);
                    Array.Copy(recordParameters.LastName.ToCharArray(), lastNameChars, recordParameters.LastName.Length);

                    writer.Write(Encoding.Unicode.GetBytes(firstNameChars), 0, maxStringLength);
                    writer.Write(Encoding.Unicode.GetBytes(lastNameChars), 0, maxStringLength);

                    writer.Write(recordParameters.DateOfBirth.Year);
                    writer.Write(recordParameters.DateOfBirth.Month);
                    writer.Write(recordParameters.DateOfBirth.Day);
                    writer.Write(Encoding.Unicode.GetBytes(new char[] { recordParameters.Gender }));
                    writer.Write(recordParameters.Experience);
                    writer.Write(recordParameters.Salary);

                    break;
                }
                else
                {
                    fileStream.Seek(recordSize - sizeof(int), SeekOrigin.Current);
                }
            }
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            int offset = 6;
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            fileStream.Seek(offset, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(fileStream);

            while (fileStream.Position < fileStream.Length)
            {
                string currentRecordFirstName = Encoding.Unicode.GetString(reader.ReadBytes(maxStringLength)).Trim('\0');
                if (currentRecordFirstName.Equals(firstName, StringComparison.InvariantCultureIgnoreCase))
                {
                    fileStream.Seek(-(offset + maxStringLength), SeekOrigin.Current);
                    fileStream.Seek(2, SeekOrigin.Current);

                    int id = reader.ReadInt32();
                    firstName = Encoding.Unicode.GetString(reader.ReadBytes(maxStringLength)).Trim('\0');
                    string lastName = Encoding.Unicode.GetString(reader.ReadBytes(maxStringLength)).Trim('\0');

                    int year = reader.ReadInt32();
                    int month = reader.ReadInt32();
                    int day = reader.ReadInt32();
                    char gender = Encoding.Unicode.GetChars(reader.ReadBytes(2))[0];
                    short experience = reader.ReadInt16();
                    decimal salary = reader.ReadDecimal();

                    FileCabinetRecord record = new FileCabinetRecord() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = new DateTime(year, month, day), Gender = gender, Experience = experience, Salary = salary };
                    records.Add(record);
                }
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            List<FileCabinetRecord> records = new List<FileCabinetRecord>();
            fileStream.Seek(0, SeekOrigin.Begin);
            BinaryReader reader = new BinaryReader(fileStream);

            while (fileStream.Position != fileStream.Length)
            {
                fileStream.Seek(2, SeekOrigin.Current);

                int id = reader.ReadInt32();
                string firstName = Encoding.Unicode.GetString(reader.ReadBytes(maxStringLength)).Trim('\0');
                string lastName = Encoding.Unicode.GetString(reader.ReadBytes(maxStringLength)).Trim('\0');
                int year = reader.ReadInt32();
                int month = reader.ReadInt32();
                int day = reader.ReadInt32();
                char gender = Encoding.Unicode.GetChars(reader.ReadBytes(2))[0];
                short experience = reader.ReadInt16();
                decimal salary = reader.ReadDecimal();

                FileCabinetRecord record = new FileCabinetRecord() { Id = id, FirstName = firstName, LastName = lastName, DateOfBirth = new DateTime(year, month, day), Gender = gender, Experience = experience, Salary = salary };
                records.Add(record);
            }

            return new ReadOnlyCollection<FileCabinetRecord>(records);
        }

        public int GetStat()
        {
            long recordsCount = fileStream.Length / recordSize;

            if (recordsCount > int.MaxValue)
            {
                throw new ArgumentOutOfRangeException($"Number of records is bigger than int.MaxValue.");
            }

            return (int)recordsCount;
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
