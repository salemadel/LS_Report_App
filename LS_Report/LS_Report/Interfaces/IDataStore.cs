using LS_Report.Models;
using System.Collections.Generic;

namespace LS_Report.Interfaces
{
    public interface IDataStore
    {
        IEnumerable<Stored_Data_Model> GetDataStoredJson(string type);

        void AddData(Stored_Data_Model data);

        void UpdateData(string type, string json);

        IEnumerable<NewContactToUpload_Model> GetContactsToUpload();

        void AddContactToUpload(NewContactToUpload_Model data);

        void UpdateContactToUpload(int id, string error);

        void DeleteContactToUpdate(int id);

        IEnumerable<EditContactToUpload_Model> GetEditContactsToUpload();

        void AddEditContactToUpload(EditContactToUpload_Model data);

        void UpdateEditContactToUpload(int id, string error);

        void DeleteEditContactToUpdate(int id);

        IEnumerable<LocationsBackGround_Model> GetLocations();

        void AddLocation(LocationsBackGround_Model data);

        void DeleteLocation(int id);

        IEnumerable<ReportToUpload_Model> GetReportsToUpload();

        void AddReportToUpload(ReportToUpload_Model data);

        void DeleteReportToUload(int id);

        void UpdateReportToUpload(int id, string error);

        IEnumerable<UnvailibilityToUpload_Model> GetUnvailibilityToUpload();

        void AddUnvailibilityToUpload(UnvailibilityToUpload_Model data);

        void UpdateUnvailibilityToUpload(int id, string error);

        void DeleteUnvailibilityToUload(int id);

        IEnumerable<Mails_Model> GetMails();

        void AddMails(Mails_Model data);

        void UpdateMails(int id, string Json);

        IEnumerable<QuastionnairesToUpload_Model> GetQuastionnairesToUpload();

        void AddQuastionnairesToUpload(QuastionnairesToUpload_Model data);

        void UpdateQuestionnairesToUpload(int id, string error);

        void DeleteQuestionnaireToUpload(int id);
    }
}