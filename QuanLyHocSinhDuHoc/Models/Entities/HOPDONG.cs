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
    
    public partial class HOPDONG
    {
        public int id { get; set; }
        public string TrangThai { get; set; }
        public Nullable<System.DateTime> NgayKy { get; set; }
        public Nullable<System.DateTime> HanHopDong { get; set; }
        public string ThanhToan { get; set; }
        public string NoiDung { get; set; }
        public string NguoiLap { get; set; }
        public Nullable<int> id_HSHS { get; set; }
    }
}
