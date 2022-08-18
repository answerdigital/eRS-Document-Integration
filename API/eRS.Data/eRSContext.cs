using System;
using System.Collections.Generic;
using eRS.Models.Models;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Auditlog>(entity =>
            {
                entity.HasKey(e => e.AuditRowId)
                    .HasName("PK__auditlog__3AD1036998B56989");

                entity.ToTable("auditlog");

                entity.HasIndex(e => e.EventDttm, "Indx_Event_Dttm");

                entity.Property(e => e.AuditRowId).HasColumnName("Audit_RowID");

                entity.Property(e => e.EventCode)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("Event_Code");

                entity.Property(e => e.EventDescription)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("Event_Description");

                entity.Property(e => e.EventDetails)
                    .HasMaxLength(500)
                    .IsUnicode(false)
                    .HasColumnName("Event_Details");

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

                entity.Property(e => e.ApptEndDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("Appt_EndDttm");

                entity.Property(e => e.ApptStDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("Appt_StDttm");

                entity.Property(e => e.RecExpiryDttm)
                    .HasColumnType("datetime")
                    .HasColumnName("rec_ExpiryDttm");

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

                entity.Property(e => e.RefDocSrlno).HasColumnName("RefDoc_srlno");

                entity.Property(e => e.RefDocStatus)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.RefDocUniqueId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RefDoc_UniqueID");

                entity.Property(e => e.RefReqUniqueId)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("RefReq_UniqueID");

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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
