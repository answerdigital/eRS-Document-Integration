using System;
using System.Collections.Generic;
using eRS.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace eRS.Data
{
    public partial class eRSContext : DbContext
    {
        public eRSContext()
        {
        }

        public eRSContext(DbContextOptions<eRSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Auditlog> Auditlogs { get; set; } = null!;
        public virtual DbSet<ErsRefReqDetail> ErsRefReqDetails { get; set; } = null!;
        public virtual DbSet<ErsdocAttachment> ErsdocAttachments { get; set; } = null!;
        public virtual DbSet<ErseventErrorlog> ErseventErrorlogs { get; set; } = null!;
        public virtual DbSet<WfsHistory> WfsHistories { get; set; } = null!;
        public virtual DbSet<WfsMaster> WfsMasters { get; set; } = null!;
        public virtual DbSet<Patient> Patients { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auditlog>(entity =>
            {
                entity.HasKey(e => e.AuditRowId)
                    .HasName("PK__auditlog__3AD1036998B56989");

                entity.ToTable("auditlog");

                entity.HasIndex(e => e.EventDttm, "Indx_Event_Dttm");

                entity.HasIndex(e => new { e.RefDocRowId, e.RefReqRowId }, "Indx_auditlog_rowid");

                entity.HasIndex(e => new { e.ErstrnsUid, e.DoctrnsUid }, "Indx_auditlog_uid");

                entity.Property(e => e.AuditRowId).HasColumnName("Audit_RowID");

                entity.Property(e => e.RefDocRowId).HasColumnName("RefDoc_RowID");

                entity.Property(e => e.RefReqRowId).HasColumnName("RefReq_RowID");

                entity.HasOne(a => a.ErsRefReqDetail)
                    .WithMany(r => r.Audits)
                    .HasForeignKey(a => a.RefReqRowId);

                entity.HasOne(a => a.ErsdocAttachment)
                    .WithMany(r => r.Audits)
                    .HasForeignKey(a => a.RefDocRowId);

                entity.Property(e => e.DoctrnsUid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Doctrns_uid");

                entity.Property(e => e.ErstrnsUid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Erstrns_uid");

                entity.Property(e => e.FromEventCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("From_Event_Code");

                entity.Property(e => e.FromStatusComments)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("From_Status_Comments");

                entity.Property(e => e.ToEventCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("To_Event_Code");

                entity.Property(e => e.ToStatusComments)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("To_Status_Comments");

                entity.Property(e => e.EventDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("Event_Dttm");

                entity.Property(e => e.RecInserted)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_inserted");

                entity.Property(e => e.RecInsertedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_insertedBy");

                entity.Property(e => e.RecStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("rec_status")
                    .IsFixedLength();
            });

            modelBuilder.Entity<ErsRefReqDetail>(entity =>
            {
                entity.HasKey(e => e.RefReqRowId)
                    .HasName("PK__ersRefRe__A3F1B4DA8E70022A");

                entity.ToTable("ersRefReq_Details");

                entity.HasIndex(e => e.RefReqUniqueId, "Indx_RefReq_UniqueID");

                entity.Property(e => e.RefReqRowId).HasColumnName("RefReq_RowID");

                entity.HasMany(d => d.WfsHistoryList)
                    .WithOne(w => w.ErsRefReqDetail)
                    .HasForeignKey(w => w.RefReqRowId);

                entity.HasMany(r => r.Audits)
                    .WithOne(a => a.ErsRefReqDetail)
                    .HasForeignKey(a => a.RefReqRowId);

                entity.HasOne(r => r.Patient)
                    .WithOne(p => p.ErsRefReqDetail)
                    .HasPrincipalKey<ErsRefReqDetail>(r => r.RefReqUbrn)
                    .HasForeignKey<Patient>(p => p.PatUbrn);

                entity.HasMany(r => r.ErsdocAttachments)
                    .WithOne(a => a.RefReqDetail)
                    .HasForeignKey(a => a.RefrequestRowId);

                entity.Property(e => e.ApptEndDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("Appt_EndDttm");

                entity.Property(e => e.ApptStDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("Appt_StDttm");

                entity.Property(e => e.RecExpiryDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_ExpiryDttm");

                entity.Property(e => e.RecInsertedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_insertedBy");

                entity.Property(e => e.RecStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("rec_status")
                    .IsFixedLength();

                entity.Property(e => e.RecUpdated)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_Updated");

                entity.Property(e => e.RecUpdatedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_UpdatedBy");

                entity.Property(e => e.RefReqFullUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_FullURL");

                entity.Property(e => e.RefReqIntent)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_intent");

                entity.Property(e => e.RefReqNhsno)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_NHSNo");

                entity.Property(e => e.RefReqNoofdocs).HasColumnName("RefReq_Noofdocs");

                entity.Property(e => e.RefReqPriority)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_Priority");

                entity.Property(e => e.RefReqSpecialty)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_Specialty");

                entity.Property(e => e.RefReqStatus)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_Status");

                entity.Property(e => e.RefReqTrustNacs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_TrustNACS");

                entity.Property(e => e.RefReqUbrn)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_UBRN");

                entity.Property(e => e.RefReqUniqueId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_UniqueID");

                entity.Property(e => e.WfsCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("wfs_Code");
            });

            modelBuilder.Entity<ErsdocAttachment>(entity =>
            {
                entity.HasKey(e => e.RefDocRowId)
                    .HasName("PK__ersdoc_a__4D61366AAD22D0BC");

                entity.ToTable("ersdoc_attachments");

                entity.HasIndex(e => e.RefDocUniqueId, "Indx_RefDoc_UniqueID");

                entity.HasIndex(e => new { e.RefrequestRowId, e.RefDocRowId }, "Indx_RefReq_RowID");

                entity.Property(e => e.RefDocRowId).HasColumnName("RefDoc_RowID");

                entity.HasMany(a => a.WfsHistoryList)
                    .WithOne(w => w.ErsdocAttachment)
                    .HasForeignKey(w => w.RefDocRowId);

                entity.HasMany(r => r.Audits)
                    .WithOne(a => a.ErsdocAttachment)
                    .HasForeignKey(r => r.RefDocRowId);

                entity.HasOne(a => a.RefReqDetail)
                    .WithMany(r => r.ErsdocAttachments)
                    .HasForeignKey(a => a.RefrequestRowId);

                entity.Property(e => e.AttachContentType)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Attach_ContentType");

                entity.Property(e => e.AttachCrtdDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("Attach_CrtdDTTM");

                entity.Property(e => e.AttachId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Attach_ID");

                entity.Property(e => e.AttachInsertedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Attach_InsertedBy");

                entity.Property(e => e.AttachSize)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("Attach_Size");

                entity.Property(e => e.AttachTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Attach_Title");

                entity.Property(e => e.AttachUrl)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Attach_URL");

                entity.Property(e => e.DocDownloadUrl)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DocDownloadURL");

                entity.Property(e => e.DocLocationUri)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("DocLocationURI");

                entity.Property(e => e.RecInsertedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_insertedBy");

                entity.Property(e => e.RecStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("rec_status")
                    .IsFixedLength();

                entity.Property(e => e.PreviouslyDownloadedDoc)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("PreviouslyDownloadedDoc")
                    .IsFixedLength();

                entity.Property(e => e.RecUpdated)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_Updated");

                entity.Property(e => e.RecUpdatedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_UpdatedBy");

                entity.Property(e => e.RefDocSrlno).HasColumnName("RefDoc_srlno");

                entity.Property(e => e.RefDocStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RefDocUniqueId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RefDoc_UniqueID");

                entity.Property(e => e.RefrequestRowId).HasColumnName("Refrequest_RowID");
            });

            modelBuilder.Entity<ErseventErrorlog>(entity =>
            {
                entity.HasKey(e => e.ERseventRowId)
                    .HasName("PK__ersevent__56276C39258039AA");

                entity.ToTable("ersevent_errorlog");

                entity.HasIndex(e => e.RecInserted, "Indx_rec_inserted");

                entity.Property(e => e.ERseventRowId).HasColumnName("eRSEvent_RowID");

                entity.Property(e => e.DoctrnsUid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Doctrns_uid");

                entity.Property(e => e.ERsEvent)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("eRS_event");

                entity.Property(e => e.ERsGetUri)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("eRS_GET_URI");

                entity.Property(e => e.ERseventResponseCode)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("eRSEvent_ResponseCode");

                entity.Property(e => e.ERseventResponseDesc)
                    .HasMaxLength(250)
                    .IsUnicode(false)
                    .HasColumnName("eRSEvent_ResponseDesc");

                entity.Property(e => e.EnsSessionId).HasColumnName("ens_SessionID");

                entity.Property(e => e.ErstrnsUid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("erstrns_uid");

                entity.Property(e => e.RecInserted)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_inserted");

                entity.Property(e => e.RecInsertedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_insertedBy");

                entity.Property(e => e.RecStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("rec_status")
                    .IsFixedLength();

                entity.Property(e => e.RefDocRowId).HasColumnName("RefDoc_RowID");

                entity.Property(e => e.RefReqRowId).HasColumnName("RefReq_RowID");
            });

            modelBuilder.Entity<WfsHistory>(entity =>
            {
                entity.HasKey(e => e.ErsdocsRowid)
                    .HasName("PK__wfs_hist__AF118B270D09ADF0");

                entity.ToTable("wfs_history");

                entity.HasIndex(e => new { e.RefDocRowId, e.RefReqRowId }, "Indx_wfds_rowid");

                entity.HasIndex(e => new { e.ErstrnsUid, e.DoctrnsUid }, "Indx_wfds_uid");

                entity.Property(e => e.ErsdocsRowid).HasColumnName("ersdocs_rowid");

                entity.Property(e => e.DoctrnsUid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("Doctrns_uid");

                entity.Property(e => e.ErstrnsUid)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("erstrns_uid");

                entity.Property(e => e.RecInserted)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_inserted");

                entity.Property(e => e.RecInsertedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_insertedBy");

                entity.Property(e => e.RecStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("rec_status")
                    .IsFixedLength();

                entity.Property(e => e.RecUpdated)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_Updated");

                entity.Property(e => e.RecUpdatedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_UpdatedBy");

                entity.Property(e => e.RefDocRowId).HasColumnName("RefDoc_RowID");

                entity.Property(e => e.RefReqRowId).HasColumnName("RefReq_RowID");

                entity.HasOne(w => w.ErsRefReqDetail)
                    .WithMany(r => r.WfsHistoryList)
                    .HasForeignKey(w => w.RefReqRowId);

                entity.HasOne(w => w.ErsdocAttachment)
                    .WithMany(d => d.WfsHistoryList)
                    .HasForeignKey(w => w.RefDocRowId);

                entity.Property(e => e.StatusCancelledBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("status_CancelledBy");

                entity.Property(e => e.StatusCancelledDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("status_CancelledDttm");

                entity.Property(e => e.StatusCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("status_Code");

                entity.HasOne(h => h.WfsMaster)
                    .WithMany(m => m.WfsHistories)
                    .HasPrincipalKey(m => m.WfsmCode)
                    .HasForeignKey(h => h.StatusCode);

                entity.Property(e => e.StatusComments)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("status_comments");

                entity.Property(e => e.StatusEffdttm)
                    .HasColumnType("datetime")
                    .HasColumnName("status_Effdttm");

                entity.Property(e => e.StatusHierarchy).HasColumnName("status_Hierarchy");

                entity.Property(e => e.StatusPerformedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("status_PerformedBy");
            });

            modelBuilder.Entity<WfsMaster>(entity =>
            {
                entity.HasKey(e => e.WfsmRowid)
                    .HasName("PK__wfs_mast__82E8B14819B8C7EC");

                entity.ToTable("wfs_master");

                entity.HasIndex(e => e.WfsmCode, "Indx_wfsm_Code");

                entity.Property(e => e.WfsmRowid).HasColumnName("wfsm_rowid");

                entity.Property(e => e.RecInserted)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_inserted");

                entity.Property(e => e.RecInsertedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_insertedBy");

                entity.Property(e => e.RecPurgeDays).HasColumnName("rec_PurgeDays");

                entity.Property(e => e.RecStatus)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("rec_status")
                    .IsFixedLength();

                entity.Property(e => e.RecUpdated)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_Updated");

                entity.Property(e => e.RecUpdatedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_UpdatedBy");

                entity.Property(e => e.WfsmCode)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("wfsm_Code");

                entity.HasMany(m => m.WfsHistories)
                    .WithOne(h => h.WfsMaster)
                    .HasPrincipalKey(m => m.WfsmCode)
                    .HasForeignKey(h => h.StatusCode);

                entity.Property(e => e.WfsmDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("wfsm_Description");

                entity.Property(e => e.WfsmDisplayValue)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("wfsm_DisplayValue");

                entity.Property(e => e.WfsmHierarchy).HasColumnName("wfsm_Hierarchy");

                entity.Property(e => e.WfsmNextHierarchy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("wfsm_NextHierarchy");

                entity.Property(e => e.WfsmPrevHierarchy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("wfsm_PrevHierarchy");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.HasKey(e => e.PatRowID)
                    .HasName("PK__patients__979035C4621E421B");

                entity.ToTable("patients");

                entity.Property(e => e.PatRowID).HasColumnName("pat_rowID");

                entity.HasOne(p => p.ErsRefReqDetail)
                    .WithOne(r => r.Patient)
                    .HasPrincipalKey<Patient>(p => p.PatUbrn)
                    .HasForeignKey<ErsRefReqDetail>(r => r.RefReqUbrn);

                entity.Property(e => e.PatUbrn)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("pat_ubrn");

                entity.Property(e => e.PatMrn)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("pat_mrn");

                entity.Property(e => e.PatNhs)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("pat_nhs");

                entity.Property(e => e.PatFamilyName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("pat_familyName");

                entity.Property(e => e.PatGivenName)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("pat_givenName");

                entity.Property(e => e.PatFullName)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasColumnName("pat_fullName");

                entity.Property(e => e.PatSex)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("pat_sex")
                    .IsFixedLength();

                entity.Property(e => e.PatDob)
                    .HasColumnType("datetime")
                    .HasColumnName("pat_dob");

                entity.Property(e => e.PatAddressOne)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("pat_addressOne");

                entity.Property(e => e.PatAddressTwo)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("pat_addressTwo");

                entity.Property(e => e.PatAddressThree)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("pat_addressThree");

                entity.Property(e => e.PatPostCode)
                    .HasMaxLength(8)
                    .IsUnicode(false)
                    .HasColumnName("pat_PostCode");

                entity.Property(e => e.PatContactNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("pat_contactNumber");

                entity.Property(e => e.PatSpeciality)
                    .HasMaxLength(35)
                    .IsUnicode(false)
                    .HasColumnName("pat_speciality");

                entity.Property(e => e.RecUpdated)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_Updated");

                entity.Property(e => e.RecUpdatedBy)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("rec_UpdatedBy");

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
