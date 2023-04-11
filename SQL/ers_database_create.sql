CREATE DATABASE ers_database;

USE ers_database;

CREATE TABLE wfs_master(
wfsm_rowid	       		integer IDENTITY(1, 1) PRIMARY KEY,
wfsm_Code				Varchar(20),
wfsm_Description 		Varchar(100),
wfsm_DisplayValue		Varchar(40),
wfsm_Hierarchy			int,
wfsm_PrevHierarchy		Varchar(15),
wfsm_NextHierarchy		Varchar(15),
rec_status	      		char(1),
rec_Updated				datetime,
rec_UpdatedBy 			Varchar(255),
rec_inserted			datetime,
rec_insertedBy			Varchar(255),
rec_PurgeDays			int
);
​
CREATE INDEX Indx_wfsm_Code ON wfs_master (wfsm_Code);
​
CREATE TABLE wfs_history(
ersdocs_rowid 			integer IDENTITY(1, 1) PRIMARY KEY,
RefReq_RowID			integer,
RefDoc_RowID			integer,
erstrns_uid				Varchar(50),
Doctrns_uid				Varchar(50),
status_Code				Varchar(20),
status_Hierarchy		int,
status_comments			Varchar(100),
status_Effdttm			datetime,
status_PerformedBy		Varchar(255),
status_CancelledDttm	datetime,
status_CancelledBy		Varchar(255),
rec_status	      		char(1),
rec_Updated				datetime,
rec_UpdatedBy 			Varchar(255),
rec_inserted			datetime,
rec_insertedBy			Varchar(255)
);
​
CREATE INDEX Indx_wfds_uid ON wfs_history (erstrns_uid,Doctrns_uid);
CREATE INDEX Indx_wfds_rowid ON wfs_history (RefDoc_RowID,RefReq_RowID);
​
CREATE TABLE ersRefReq_Details(
RefReq_RowID		integer IDENTITY(1, 1) PRIMARY KEY,
RefReq_UniqueID		Varchar(50),
RefReq_NHSNo		Varchar(15),
RefReq_UBRN			Varchar(35),
RefReq_TrustNACS	Varchar(10),
Appt_StDttm			datetime,
Appt_EndDttm		datetime,
RefReq_Specialty	Varchar(35),
RefReq_Status		Varchar(35),
RefReq_intent		Varchar(35),
RefReq_Priority		Varchar(35),
RefReq_Noofdocs		integer,
RefReq_FullURL		Varchar(250),
wfs_Code			Varchar(20),
rec_ExpiryDttm		datetime,
rec_status	      	char(1),
rec_Updated			datetime,
rec_UpdatedBy 		Varchar(255),
rec_insertedBy		Varchar(255)
);
​
CREATE INDEX Indx_RefReq_UniqueID ON ersRefReq_Details (RefReq_UniqueID);
​
CREATE TABLE ersdoc_attachments(
RefDoc_RowID			integer IDENTITY(1, 1) PRIMARY KEY,
Refrequest_RowID		integer,
RefDoc_srlno			integer,
RefDoc_UniqueID			Varchar(50),
RefDocStatus			Varchar(20),
Attach_ID				Varchar(20),
Attach_InsertedBy		Varchar(255),
Attach_ContentType		Varchar(15),
Attach_URL				Varchar(250),
Attach_Size				Varchar(15),
Attach_Title			Varchar(50),
Attach_CrtdDTTM			datetime,
DocDownloadURL			Varchar(250),
DocLocationURI			Varchar(250),
rec_status	      		char(1),
rec_Updated				datetime,
rec_UpdatedBy 			Varchar(255),
rec_insertedBy			Varchar(255),
PreviouslyDownloadedDoc char(1)
);
​
CREATE INDEX Indx_RefReq_RowID ON ersdoc_attachments (Refrequest_RowID,RefDoc_RowID);
CREATE INDEX Indx_RefDoc_UniqueID ON ersdoc_attachments (RefDoc_UniqueID);
​
CREATE TABLE ersevent_errorlog(
eRSEvent_RowID				integer IDENTITY(1, 1) PRIMARY KEY,
eRS_event					Varchar(10),
RefReq_RowID				integer,
RefDoc_RowID				integer,
erstrns_uid					Varchar(50),
Doctrns_uid					Varchar(50),
eRS_GET_URI					Varchar(150),
eRSEvent_ResponseCode		Varchar(30),
eRSEvent_ResponseDesc		Varchar(250),
ens_SessionID				integer,
rec_status	      			char(1),
rec_inserted				datetime,
rec_insertedBy				Varchar(255)
);
​
CREATE INDEX Indx_rec_inserted ON ersevent_errorlog (rec_inserted);
​
​
CREATE TABLE auditlog(
Audit_RowID                 integer IDENTITY(1, 1) PRIMARY KEY,
Event_Dttm                  datetime,
From_Event_Code             Varchar(20),
From_Status_Comments        Varchar(100),
To_Event_Code               Varchar(20),
To_Status_Comments          Varchar(100),
rec_status                  char(1),
rec_inserted                datetime,
rec_insertedBy              Varchar(255),
RefReq_RowID                integer,
RefDoc_RowID                integer,
Erstrns_uid                 Varchar(50),
Doctrns_uid                 Varchar(50)
);
​
CREATE INDEX Indx_Event_Dttm ON auditlog (Event_Dttm);
​
​
CREATE TABLE patients(
pat_rowID           integer IDENTITY(1,1) PRIMARY KEY,
pat_ubrn            Varchar(35),
pat_mrn             Varchar(10),
pat_nhs             Varchar(15),
pat_familyName      Varchar(30),
pat_givenName       Varchar(30),
pat_sex             char(1),
pat_dob             datetime,
pat_addressOne      Varchar(30),
pat_addressTwo      Varchar(30),
pat_addressThree    Varchar(30),
pat_PostCode        Varchar(8),
pat_contactNumber   Varchar(20),
pat_speciality      Varchar(35),
rec_Updated         datetime,
rec_UpdatedBy       Varchar(255),
pat_fullName        as concat(pat_givenName, ' ', pat_familyName)
);
​
CREATE INDEX Indx_Ubrn ON patients (pat_ubrn);
​
​
CREATE TABLE users(
user_rowID      integer IDENTITY(1,1) PRIMARY KEY,
user_reference  uniqueidentifier,
user_email      Varchar(255),
user_password   Varchar(300),
user_surname    Varchar(30),
user_forename   Varchar(30)
);
​
CREATE INDEX Indx_Email ON users (user_email);


INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('D-ERSDL-SUCC','Document Downloaded from eRS to Trust Network successfully.','Awaiting Upload to EPR',1,'0','3#4','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('D-ERSDL-FAIL','Document Download failed.','Failed to Download',2,'0','0','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('D-ULEPR-SUCC','Uploaded to EPR','Uploaded to EPR',3,'1','5#6','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('D-ULEPR-FAIL','Technical issue in uploading EPR','Failed to upload to EPR',4,'1','1','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('D-QCEPR-SUCC','Quality Check Success & Document in EPR','Quality Check Success',5,'3','0','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('D-QCEPR-FAIL','Quality Check Failed','Quality Check Failed',6,'3','3','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);

INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('R-ERSDL-SUCC','Attached Documents downloaded successfully.','Awaiting Upload to EPR',7,'0','9#10','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('R-ERSDL-FAIL','Attached Documents partially downloaded or missing.','Attachment Downloads Incomplete',8,'0','0','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('R-ULEPR-SUCC','Uploaded to EPR','Uploaded to EPR',9,'7','11#12','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('R-ULEPR-FAIL','Technical issue in uploading EPR','Failed to upload to EPR',10,'7','7','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('R-QCEPR-SUCC','Quality Check Success & Document in EPR','Quality Check Success',11,'9','0','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
INSERT INTO wfs_master(wfsm_code, wfsm_description, wfsm_displayvalue, wfsm_hierarchy, wfsm_prevhierarchy, wfsm_nexthierarchy, rec_status, rec_updated, rec_updatedby, rec_inserted, rec_insertedby,rec_PurgeDays) VALUES('R-QCEPR-FAIL','Quality Check Failed','Quality Check Failed',12,'9','9','A', GETDATE(), 'Admin', GETDATE(), 'Admin',0);
