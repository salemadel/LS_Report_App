using LS_Report.Interfaces;
using LS_Report.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;

namespace LS_Report.Data
{
    public  class DataStore : IDataStore
    {
        private SQLiteConnection _connection;

        public DataStore()
        {

                _connection = StaticSqlConnection.GetConnection();
               
        }
        

        public IEnumerable<Stored_Data_Model> GetDataStoredJson(string type)
        {
           
                return _connection.Query<Stored_Data_Model>("Select * From Stored_Data_Model Where Type = ?", type);
            
        }

        public void AddData(Stored_Data_Model data)
        {
            if(_connection != null)
            _connection.Insert(data);
        }

        public void UpdateData(string type, string json)
        {
            if (_connection != null)
                _connection.Execute("Update Stored_Data_Model Set Json = ? , DateTime = ? Where Type = ?", json, DateTime.Now, type);
        }

        public IEnumerable<NewContactToUpload_Model> GetContactsToUpload()
        {
            return _connection.Table<NewContactToUpload_Model>();
        }

        public void AddContactToUpload(NewContactToUpload_Model data)
        {
            if (_connection != null)
                _connection.Insert(data);
        }

        public void UpdateContactToUpload(int id, string error)
        {
            if (_connection != null)
                _connection.Execute("Update NewContactToUpload_Model Set Last_Error = ? , Last_Sync = ? Where Id = ?", error, DateTime.Now, id);
        }

        public void DeleteContactToUpdate(int id)
        {
            if (_connection != null)
                _connection.Execute("Delete From NewContactToUpload_Model Where Id = ?", id);
        }

        public IEnumerable<EditContactToUpload_Model> GetEditContactsToUpload()
        {
            return _connection.Table<EditContactToUpload_Model>();
        }

        public void AddEditContactToUpload(EditContactToUpload_Model data)
        {
            if (_connection != null)
                _connection.Insert(data);
        }

        public void UpdateEditContactToUpload(int id, string error)
        {
            if (_connection != null)
                _connection.Execute("Update EditContactToUpload_Model Set Last_Error = ? , Last_Sync = ? Where Id = ?", error, DateTime.Now, id);
        }

        public void DeleteEditContactToUpdate(int id)
        {
            _connection.Execute("Delete From EditContactToUpload_Model Where Id = ?", id);
        }

        public IEnumerable<LocationsBackGround_Model> GetLocations()
        {
            return _connection.Table<LocationsBackGround_Model>();
        }

        public void AddLocation(LocationsBackGround_Model data)
        {
            if (_connection != null)
                _connection.Insert(data);
        }

        public void DeleteLocation(int id)
        {
            if (_connection != null)
                _connection.Execute("Delete From LocationsBackGround_Model Where Id = ?", id);
        }

        public IEnumerable<ReportToUpload_Model> GetReportsToUpload()
        {
            return _connection.Table<ReportToUpload_Model>();
        }

        public void AddReportToUpload(ReportToUpload_Model data)
        {
            if (_connection != null)
                _connection.Insert(data);
        }

        public void UpdateReportToUpload(int id, string error)
        {
            if (_connection != null)
                _connection.Execute("Update ReportToUpload_Model Set Last_Error = ? , Last_Sync = ? Where Id = ?", error, DateTime.Now, id);
        }

        public void DeleteReportToUload(int id)
        {
            if (_connection != null)
                _connection.Execute("Delete From ReportToUpload_Model Where Id = ?", id);
        }

        public IEnumerable<UnvailibilityToUpload_Model> GetUnvailibilityToUpload()
        {
            return _connection.Table<UnvailibilityToUpload_Model>();
        }

        public void AddUnvailibilityToUpload(UnvailibilityToUpload_Model data)
        {
            if (_connection != null)
                _connection.Insert(data);
        }

        public void UpdateUnvailibilityToUpload(int id, string error)
        {
            if (_connection != null)
                _connection.Execute("Update UnvailibilityToUpload_Model Set Last_Error = ? , Last_Sync = ? Where Id = ?", error, DateTime.Now, id);
        }

        public void DeleteUnvailibilityToUload(int id)
        {
            if (_connection != null)
                _connection.Execute("Delete From UnvailibilityToUpload_Model Where Id = ?", id);
        }

        public IEnumerable<Mails_Model> GetMails()
        {
            
                return _connection.Table<Mails_Model>();
        }

        public void AddMails(Mails_Model data)
        {
            if (_connection != null)
                _connection.Insert(data);
        }

        public void UpdateMails(int id, string Json)
        {
            if (_connection != null)
                _connection.Execute("Update Mails_Model Set Json = ? Where Id = ?", Json, id);
        }
        public IEnumerable<QuastionnairesToUpload_Model> GetQuastionnairesToUpload()
        {

            return _connection.Table<QuastionnairesToUpload_Model>();
        }

        public void AddQuastionnairesToUpload(QuastionnairesToUpload_Model data)
        {
            if (_connection != null)
                _connection.Insert(data);
        }

        public void UpdateQuestionnairesToUpload(int id, string error)
        {
            if (_connection != null)
                _connection.Execute("Update QuastionnairesToUpload_Model Set Last_Error = ? , Last_Sync = ? Where Id = ?", error, DateTime.Now, id);
        }
        public void DeleteQuestionnaireToUpload(int id)
        {
            if (_connection != null)
                _connection.Execute("Delete From QuastionnairesToUpload_Model Where Id = ?", id);
        }
    }
}