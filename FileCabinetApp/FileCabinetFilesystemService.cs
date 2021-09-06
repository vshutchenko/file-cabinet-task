using System;
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
            int id = (int)(fileStream.Length == 0 ? 1 : fileStream.Length / recordSize);
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

            return id;
        }

        public void EditRecord(int id, RecordParameters recordParameters)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            throw new NotImplementedException();
        }

        public int GetStat()
        {
            throw new NotImplementedException();
        }

        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            throw new NotImplementedException();
        }
    }
}
