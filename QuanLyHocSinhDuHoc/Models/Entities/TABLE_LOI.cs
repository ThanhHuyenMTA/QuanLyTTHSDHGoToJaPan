//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyHocSinhDuHoc.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class TABLE_LOI
    {
        public int id { get; set; }
        public Nullable<int> id_HS { get; set; }
        public string So_CMT { get; set; }
        public Nullable<int> id_BTN { get; set; }
        public Nullable<int> id_GKS { get; set; }
        public Nullable<int> id_HB { get; set; }
        public string TypeLOI { get; set; }
        public Nullable<System.DateTime> TimeStart { get; set; }
        public Nullable<System.DateTime> TimeEnd { get; set; }
        public string TrangThai { get; set; }
        public Nullable<int> NguoiSua { get; set; }
    }
}
