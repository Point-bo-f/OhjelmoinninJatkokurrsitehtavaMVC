﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OhjelmoinninJatkokurssiMVC.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class AsiakastietokantaEntities : DbContext
    {
        public AsiakastietokantaEntities()
            : base("name=AsiakastietokantaEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Henkilot> Henkilot { get; set; }
        public virtual DbSet<Projektit> Projektit { get; set; }
        public virtual DbSet<Tunnit> Tunnit { get; set; }
    }
}
