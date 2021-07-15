using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.ValueGeneration;

#nullable disable

namespace MyLabLocalizer.LocalizationService.Entities
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

        public virtual DbSet<LocConcept2Context> LocConcept2Contexts { get; set; }
        public virtual DbSet<LocConceptsTable> LocConceptsTables { get; set; }
        public virtual DbSet<LocContext> LocContexts { get; set; }
        public virtual DbSet<LocJob2Concept> LocJob2Concepts { get; set; }
        public virtual DbSet<LocJobList> LocJobLists { get; set; }
        public virtual DbSet<LocLanguage> LocLanguages { get; set; }
        public virtual DbSet<LocLoggedDatum> LocLoggedData { get; set; }
        public virtual DbSet<LocSessionDatum> LocSessionData { get; set; }
        public virtual DbSet<LocString> LocStrings { get; set; }
        public virtual DbSet<LocStringType> LocStringTypes { get; set; }
        public virtual DbSet<LocStrings2Context> LocStrings2Contexts { get; set; }
        public virtual DbSet<LocStrings2translate> LocStrings2translates { get; set; }
        public virtual DbSet<LocStringsacceptable> LocStringsacceptables { get; set; }
        public virtual DbSet<LocStringslocked> LocStringslockeds { get; set; }
        public virtual DbSet<VConceptStringToContext> VConceptStringToContexts { get; set; }
        public virtual DbSet<VJobListConcept> VJobListConcepts { get; set; }
        public virtual DbSet<VLocalization> VLocalizations { get; set; }
        public virtual DbSet<VString> VStrings { get; set; }
        public virtual DbSet<VStringsToContext> VStringsToContexts { get; set; }
        public virtual DbSet<VTranslatedConcept> VTranslatedConcepts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<LocConcept2Context>(entity =>
            {
                entity.ToTable("LOC_Concept2Context");

                entity.HasIndex(e => e.Idcontext, "Concept2Context$CONTEXTSConcept2Context");

                entity.HasIndex(e => e.Idconcept, "Concept2Context$ConceptsTableConcept2Context");

                entity.HasIndex(e => e.Idconcept, "Concept2Context$IDConcept");

                entity.HasIndex(e => e.Idcontext, "Concept2Context$IDContext");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idconcept).HasColumnName("IDConcept");

                entity.Property(e => e.Idcontext).HasColumnName("IDContext");

                entity.HasOne(d => d.IdconceptNavigation)
                    .WithMany(p => p.LocConcept2Contexts)
                    .HasForeignKey(d => d.Idconcept)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Concept2Context$ConceptsTableConcept2Context");

                entity.HasOne(d => d.IdcontextNavigation)
                    .WithMany(p => p.LocConcept2Contexts)
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
                    .HasMaxLength(255)
                    .HasColumnName("LocalizationID");

                entity.Property(e => e.SsmaTimeStamp)
                    .IsRequired()
                    .IsRowVersion()
                    .IsConcurrencyToken()
                    .HasColumnName("SSMA_TimeStamp");
            });

            modelBuilder.Entity<LocContext>(entity =>
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
                    .WithMany(p => p.LocJob2Concepts)
                    .HasForeignKey(d => d.Idconcept2Context)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOC_Job2Concept_LOC_Concept2Context");

                entity.HasOne(d => d.IdjobListNavigation)
                    .WithMany(p => p.LocJob2Concepts)
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
                    .WithMany(p => p.LocJobLists)
                    .HasForeignKey(d => d.IdisoCoding)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_LOC_JobList_LOC_Languages");
            });

            modelBuilder.Entity<LocLanguage>(entity =>
            {
                entity.ToTable("LOC_Languages");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Isocoding)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ISOCoding");

                entity.Property(e => e.LanguageName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LocLoggedDatum>(entity =>
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

            modelBuilder.Entity<LocSessionDatum>(entity =>
            {
                entity.ToTable("LOC_SessionData");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.InitSessionDate).HasColumnType("datetime");

                entity.Property(e => e.LastModify).HasColumnType("datetime");

                entity.Property(e => e.SessionId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("SessionID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(25);
            });

            modelBuilder.Entity<LocString>(entity =>
            {
                entity.ToTable("LOC_STRINGS");

                entity.HasIndex(e => e.Idlanguage, "STRINGS$IDLanguage");

                entity.HasIndex(e => e.Idtype, "STRINGS$IDType");

                entity.Property(e => e.Id)
                .HasColumnName("ID");

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

            modelBuilder.Entity<LocStringType>(entity =>
            {
                entity.ToTable("LOC_StringTypes");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LocStrings2Context>(entity =>
            {
                entity.ToTable("LOC_Strings2Context");

                entity.HasIndex(e => e.Idconcept2Context, "Strings2Context$Concept2ContextStrings2Context");

                entity.HasIndex(e => e.Idconcept2Context, "Strings2Context$IDConcept2Context");

                entity.HasIndex(e => e.Idstring, "Strings2Context$IDString");

                entity.HasIndex(e => e.Idstring, "Strings2Context$STRINGSStrings2Context");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idconcept2Context).HasColumnName("IDConcept2Context");

                entity.Property(e => e.Idstring).HasColumnName("IDString");

                entity.HasOne(d => d.Idconcept2ContextNavigation)
                    .WithMany(p => p.LocStrings2Contexts)
                    .HasForeignKey(d => d.Idconcept2Context)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Strings2Context$Concept2ContextStrings2Context");

                entity.HasOne(d => d.IdstringNavigation)
                    .WithMany(p => p.LocStrings2Contexts)
                    .HasForeignKey(d => d.Idstring)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Strings2Context$STRINGSStrings2Context");
            });

            modelBuilder.Entity<LocStrings2translate>(entity =>
            {
                entity.ToTable("LOC_STRINGS2Translate");

                entity.HasIndex(e => e.Idstring, "STRINGS2Translate$IDString");

                entity.HasIndex(e => e.Idstring, "STRINGS2Translate$STRINGSSTRINGS2Translate");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idstring).HasColumnName("IDString");

                entity.HasOne(d => d.IdstringNavigation)
                    .WithMany(p => p.LocStrings2translates)
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

                entity.HasIndex(e => e.Idstring, "STRINGSLocked$IDString");

                entity.HasIndex(e => e.Idstring, "STRINGSLocked$STRINGSSTRINGSLocked");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idstring).HasColumnName("IDString");

                entity.HasOne(d => d.IdstringNavigation)
                    .WithMany(p => p.LocStringslockeds)
                    .HasForeignKey(d => d.Idstring)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("STRINGSLocked$STRINGSSTRINGSLocked");
            });

            modelBuilder.Entity<VConceptStringToContext>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vConceptStringToContext");

                entity.Property(e => e.ComponentNamespace)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ContextName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idconcept2Context).HasColumnName("IDConcept2Context");

                entity.Property(e => e.Idcontext).HasColumnName("IDContext");

                entity.Property(e => e.InternalNamespace).HasMaxLength(50);

                entity.Property(e => e.LocalizationId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("LocalizationID");
            });

            modelBuilder.Entity<VJobListConcept>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vJobListConcept");

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

                entity.Property(e => e.JobListLanguageId).HasColumnName("jobListLanguageId");

                entity.Property(e => e.JobListUserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.String).HasMaxLength(255);

                entity.Property(e => e.StringInEnglish).HasMaxLength(255);

                entity.Property(e => e.StringType).HasMaxLength(50);
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

                entity.Property(e => e.Context).HasMaxLength(255);

                entity.Property(e => e.JobList).HasMaxLength(50);

                entity.Property(e => e.JobListUserName).HasMaxLength(50);

                entity.Property(e => e.Language).HasMaxLength(50);

                entity.Property(e => e.LanguageIsoCode).HasMaxLength(10);

                entity.Property(e => e.String).HasMaxLength(255);

                entity.Property(e => e.StringType).HasMaxLength(50);
            });

            modelBuilder.Entity<VString>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vStrings");

                entity.Property(e => e.String)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<VStringsToContext>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vStringsToContext");

                entity.Property(e => e.ComponentNamespace)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ConceptId).HasColumnName("ConceptID");

                entity.Property(e => e.ContextName)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Idtype).HasColumnName("IDType");

                entity.Property(e => e.InternalNamespace).HasMaxLength(50);

                entity.Property(e => e.Isocoding)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("ISOCoding");

                entity.Property(e => e.LocalizationId)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("LocalizationID");

                entity.Property(e => e.String)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.StringId).HasColumnName("StringID");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<VTranslatedConcept>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vTranslatedConcept");

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
