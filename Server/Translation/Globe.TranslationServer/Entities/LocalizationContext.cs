using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Globe.TranslationServer.Entities
{
    public partial class LocalizationContext : DbContext
    {
        public LocalizationContext()
        {
        }

        public LocalizationContext(DbContextOptions<LocalizationContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LocConcept2Context> LocConcept2Context { get; set; }
        public virtual DbSet<LocConceptsTable> LocConceptsTable { get; set; }
        public virtual DbSet<LocContexts> LocContexts { get; set; }
        public virtual DbSet<LocJob2Concept> LocJob2Concept { get; set; }
        public virtual DbSet<LocJobList> LocJobList { get; set; }
        public virtual DbSet<LocLanguages> LocLanguages { get; set; }
        public virtual DbSet<LocLoggedData> LocLoggedData { get; set; }
        public virtual DbSet<LocSessionData> LocSessionData { get; set; }
        public virtual DbSet<LocStringTypes> LocStringTypes { get; set; }
        public virtual DbSet<LocStrings> LocStrings { get; set; }
        public virtual DbSet<LocStrings2Context> LocStrings2Context { get; set; }
        public virtual DbSet<LocStrings2translate> LocStrings2translate { get; set; }
        public virtual DbSet<LocStringsacceptable> LocStringsacceptable { get; set; }
        public virtual DbSet<LocStringslocked> LocStringslocked { get; set; }
        public virtual DbSet<VLocalization> VLocalization { get; set; }

        //        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //        {
        //            if (!optionsBuilder.IsConfigured)
        //            {
        //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
        //                optionsBuilder.UseSqlServer("Server=PC\\SQLExpress;Database=Localization;Trusted_Connection=True;");
        //            }
        //        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocConcept2Context>(entity =>
            {
                entity.ToTable("LOC_Concept2Context");

                entity.HasIndex(e => e.Idconcept)
                    .HasName("Concept2Context$IDConcept");

                entity.HasIndex(e => e.Idcontext)
                    .HasName("Concept2Context$IDContext");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idconcept).HasColumnName("IDConcept");

                entity.Property(e => e.Idcontext).HasColumnName("IDContext");

                entity.HasOne(d => d.IdconceptNavigation)
                    .WithMany(p => p.LocConcept2Context)
                    .HasForeignKey(d => d.Idconcept)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Concept2Context$ConceptsTableConcept2Context");

                entity.HasOne(d => d.IdcontextNavigation)
                    .WithMany(p => p.LocConcept2Context)
                    .HasForeignKey(d => d.Idcontext)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Concept2Context$CONTEXTSConcept2Context");
            });

            modelBuilder.Entity<LocConceptsTable>(entity =>
            {
                entity.ToTable("LOC_ConceptsTable");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.Property(e => e.ComponentNamespace)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Ignore).HasDefaultValueSql("((0))");

                entity.Property(e => e.InternalNamespace).HasMaxLength(50);

                entity.Property(e => e.LocalizationId)
                    .IsRequired()
                    .HasColumnName("LocalizationID")
                    .HasMaxLength(255);

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRequired()
                    .HasColumnName("SSMA_TimeStamp")
                    .IsRowVersion()
                    .IsConcurrencyToken();
            });

            modelBuilder.Entity<LocContexts>(entity =>
            {
                entity.ToTable("LOC_CONTEXTS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ContextName)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<LocJob2Concept>(entity =>
            {
                entity.ToTable("LOC_Job2Concept");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idconcept2Context).HasColumnName("IDConcept2Context");

                entity.Property(e => e.IdjobList).HasColumnName("IDJobList");

                entity.HasOne(d => d.Idconcept2ContextNavigation)
                    .WithMany(p => p.LocJob2Concept)
                    .HasForeignKey(d => d.Idconcept2Context)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOC_Job2Concept_LOC_Concept2Context");

                entity.HasOne(d => d.IdjobListNavigation)
                    .WithMany(p => p.LocJob2Concept)
                    .HasForeignKey(d => d.IdjobList)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOC_Job2Concept_LOC_JobList");
            });

            modelBuilder.Entity<LocJobList>(entity =>
            {
                entity.ToTable("LOC_JobList");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdisoCoding).HasColumnName("IDIsoCoding");

                entity.Property(e => e.JobName).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.IdisoCodingNavigation)
                    .WithMany(p => p.LocJobList)
                    .HasForeignKey(d => d.IdisoCoding)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOC_JobList_LOC_Languages");
            });

            modelBuilder.Entity<LocLanguages>(entity =>
            {
                entity.ToTable("LOC_Languages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Isocoding)
                    .IsRequired()
                    .HasColumnName("ISOCoding")
                    .HasMaxLength(10);

                entity.Property(e => e.LanguageName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LocLoggedData>(entity =>
            {
                entity.ToTable("LOC_LoggedData");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LoggedString)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.SessionDataId).HasColumnName("SessionDataID");

                entity.HasOne(d => d.SessionData)
                    .WithMany(p => p.LocLoggedData)
                    .HasForeignKey(d => d.SessionDataId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("_LOC_LoggedData2LOC_SessionData");
            });

            modelBuilder.Entity<LocSessionData>(entity =>
            {
                entity.ToTable("LOC_SessionData");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InitSessionDate).HasColumnType("datetime");

                entity.Property(e => e.LastModify).HasColumnType("datetime");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasColumnName("SessionID")
                    .HasMaxLength(255);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<LocStringTypes>(entity =>
            {
                entity.ToTable("LOC_StringTypes");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LocStrings>(entity =>
            {
                entity.ToTable("LOC_STRINGS");

                entity.HasIndex(e => e.Idlanguage)
                    .HasName("STRINGS$LanguagesSTRINGS");

                entity.HasIndex(e => e.Idtype)
                    .HasName("STRINGS$StringTypesSTRINGS");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idlanguage).HasColumnName("IDLanguage");

                entity.Property(e => e.Idtype).HasColumnName("IDType");

                entity.Property(e => e.String)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.HasOne(d => d.IdlanguageNavigation)
                    .WithMany(p => p.LocStrings)
                    .HasForeignKey(d => d.Idlanguage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STRINGS$LanguagesSTRINGS");

                entity.HasOne(d => d.IdtypeNavigation)
                    .WithMany(p => p.LocStrings)
                    .HasForeignKey(d => d.Idtype)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STRINGS$StringTypesSTRINGS");
            });

            modelBuilder.Entity<LocStrings2Context>(entity =>
            {
                entity.ToTable("LOC_Strings2Context");

                entity.HasIndex(e => e.Idconcept2Context)
                    .HasName("Strings2Context$IDConcept2Context");

                entity.HasIndex(e => e.Idstring)
                    .HasName("Strings2Context$STRINGSStrings2Context");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idconcept2Context).HasColumnName("IDConcept2Context");

                entity.Property(e => e.Idstring).HasColumnName("IDString");

                entity.HasOne(d => d.Idconcept2ContextNavigation)
                    .WithMany(p => p.LocStrings2Context)
                    .HasForeignKey(d => d.Idconcept2Context)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Strings2Context$Concept2ContextStrings2Context");

                entity.HasOne(d => d.IdstringNavigation)
                    .WithMany(p => p.LocStrings2Context)
                    .HasForeignKey(d => d.Idstring)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Strings2Context$STRINGSStrings2Context");
            });

            modelBuilder.Entity<LocStrings2translate>(entity =>
            {
                entity.ToTable("LOC_STRINGS2Translate");

                entity.HasIndex(e => e.Idstring)
                    .HasName("STRINGS2Translate$STRINGSSTRINGS2Translate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idstring).HasColumnName("IDString");

                entity.HasOne(d => d.IdstringNavigation)
                    .WithMany(p => p.LocStrings2translate)
                    .HasForeignKey(d => d.Idstring)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STRINGS2Translate$STRINGSSTRINGS2Translate");
            });

            modelBuilder.Entity<LocStringsacceptable>(entity =>
            {
                entity.ToTable("LOC_STRINGSAcceptable");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IdString).HasColumnName("ID_String");
            });

            modelBuilder.Entity<LocStringslocked>(entity =>
            {
                entity.ToTable("LOC_STRINGSLocked");

                entity.HasIndex(e => e.Idstring)
                    .HasName("STRINGSLocked$STRINGSSTRINGSLocked");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idstring).HasColumnName("IDString");

                entity.HasOne(d => d.IdstringNavigation)
                    .WithMany(p => p.LocStringslocked)
                    .HasForeignKey(d => d.Idstring)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STRINGSLocked$STRINGSSTRINGSLocked");
            });

            modelBuilder.Entity<VLocalization>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vLocalization");

                entity.Property(e => e.Concept)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.ConceptComment).HasMaxLength(255);

                entity.Property(e => e.ConceptComponentNamespace)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ConceptInternalNamespace).HasMaxLength(50);

                entity.Property(e => e.Context)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.JobList).HasMaxLength(50);

                entity.Property(e => e.JobListUserName).HasMaxLength(50);

                entity.Property(e => e.Language)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LanguageIsoCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.String)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StringType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
