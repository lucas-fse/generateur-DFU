﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GenerateurDFUSafir.Models.DAL
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class x160Entities : DbContext
    {
        public x160Entities()
            : base("name=x160Entities")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ZCONFPACK4> ZCONFPACK4 { get; set; }
        public virtual DbSet<SORDER> SORDER { get; set; }
        public virtual DbSet<SQUOTE> SQUOTE { get; set; }
        public virtual DbSet<SQUOTED> SQUOTED { get; set; }
        public virtual DbSet<BPADDRESS> BPADDRESS { get; set; }
    }
}
