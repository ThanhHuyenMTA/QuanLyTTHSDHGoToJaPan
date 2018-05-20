function SaveNamHoc() {
    var namhoc = {
        ThoiGian: $('#ThoiGian').val(),
        TenLop: $('#TenLop').val(),
        TenTruong: $('#TenTruong').val()
    };
    $.ajax({
        type: "POST",
        url: "/NamHoc/SaveNamhoc",
        dataType: "json",
        data: JSON.stringify(namhoc),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data != "NO") {
                var rows = '';
                rows += '<tr><td><div class=word-wrap>' + data.ThoiGian + '</div></td>';
                rows += '<td><div class=word-wrap>' + data.TenLop + '</td>';
                rows += '<td><div class=word-wrap>' + data.TenTruong + '</div></td>';
                rows += '<td class=actions>';
                rows += '<a data-toggle=tooltip data-placement=bottom  title="Nhập điểm" href="#" onclick="LoadFormNhapDiem(' + data.id + ')">';
                rows += '<i class="fa fa-plus"></i>';
                rows += '</a>';
                rows += '</td></tr>';
                $("#tableMonhoc").append(rows);
                $('#ThoiGian').val('');
                $('#TenLop').val('');
                $('#TenTruong').val('');
            }            
        },
        error: function (err) {
            alert("Error: " + err.responseText);
        }
    });
}
function LoadFormNhapDiem(id_namhoc)
{
    $.ajax({
        type: "GET",
        url: "/KyHoc/FormNhapDiem",
        dataType: "json",
        data: { id: id_namhoc },//dua ra id_namhoc
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#Diemhocki').css('display', 'block');
            var allInputs = document.getElementsByTagName("input");
            var results = [];
            for (var x = 0; x < allInputs.length; x++)
                allInputs[x].value = '';
        },
        error: function (err) {
            alert("Error: " + err.responseText);
        }
    });
}




function NhapDiem(id_namhoc) {
    debugger;
    $.ajax({
        type: "GET",
        url: "/KyHoc/ViewNhapDiem",
        dataType: "json",
        data: { id: id_namhoc },//dua ra id_namhoc
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            $('#Diemhocki').css('display', 'block');
            var allInputs = document.getElementsByTagName("input");
            var results = [];
            for (var x = 0; x < allInputs.length; x++)
                allInputs[x].value='';
        },
        error: function (err) {
            alert("Error: " + err.responseText);
        }
    });
}
function validateDiem(start,mess)
{
    debugger;
    var inputs = document.getElementsByTagName('input');
    var i=start;
    while(i<inputs.length)
    {
        if (inputs[i].value == "" || inputs[i].value <= 0 || inputs[i].value >10) {
            $("#" + mess).text("Điểm không được để rỗng. \nĐịnh dạng điểm phải 0 < x <=10");
            return false;
        }
        i=i+3;
    }
    return true;
}
//test luu 1 lan 3 ki
function LayDiem(nameHocKy, toan, ly, hoa, sinh, tin, van, su, dia, ngoaingu, congnghe, quocphong, theduc, tuchon, gdcd, diemTb) {
    var diemky = {
        TenKiHoc: nameHocKy,
        Toan: $('#' + toan).val(),
        Ly: $('#' + ly).val(),
        Hoa: $('#' + hoa).val(),
        Sinh: $('#' + sinh).val(),
        Tin: $('#' + tin).val(),
        Van: $('#' + van).val(),
        Su: $('#' + su).val(),
        Dia: $('#' + dia).val(),
        NgoaiNgu: $('#' + ngoaingu).val(),
        CongNghe: $('#' + congnghe).val(),
        QuocPhongAnNinh: $('#' + quocphong).val(),
        TheDuc: $('#' + theduc).val(),
        TuChon: $('#' + tuchon).val(),
        GiaoDucCongDan: $('#' + gdcd).val(),
        DiemTrungBinh: $('#' + diemTb).val()
    }
    return diemky;
}
function Diem3Ky() {
    debugger;
    var bool1 = validateDiem(0, "messTbDiemI");
    var bool2 = validateDiem(1, "messTbDiemII");
    var bool3 = validateDiem(2, "messTbDiemCN");
    if (bool1 == false || bool2 == false || bool3 == false)
    {
        $("#messTbDiem").text("Điểm không được để rỗng. \nĐịnh dạng điểm phải 0 < x <=10");       
        return false;
    }else {
        var diemKyI = LayDiem("Hkỳ I", "ToanI", "LyI", "HoaI", "SinhI", "TinI", "VanI", "SuI", "DiaI", "NgoaiNguI", "CongNgheI", "QuocPhongAnNinhI", "TheDucI", "TuChonI", "GiaoDucCongDanI", "DiemTrungBinhI");
        var diemKyII = LayDiem("Hkỳ II", "ToanII", "LyII", "HoaII", "SinhII", "TinII", "VanII", "SuII", "DiaII", "NgoaiNguII", "CongNgheII", "QuocPhongAnNinhII", "TheDucII", "TuChonII", "GiaoDucCongDanII", "DiemTrungBinhII");
        var diemKyCN = LayDiem("CN", "ToanCN", "LyCN", "HoaCN", "SinhCN", "TinCN", "VanCN", "SuCN", "DiaCN", "NgoaiNguCN", "CongNgheCN", "QuocPhongAnNinhCN", "TheDucCN", "TuChonCN", "GiaoDucCongDanCN", "DiemTrungBinhCN");

        $.ajax({
            type: "POST",
            url: "/KyHoc/SaveDiem3Ky",
            dataType: "json",
            data: JSON.stringify({ diemKyI: diemKyI, diemKyII: diemKyII, diemKyCN: diemKyCN }),
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                if (data == "YES") {
                    $("#disDiemKyI").css("color", "blue");
                    $("#disDiemKyI").attr("disabled", true);
                    $("#disDiemKyII").css("color", "blue");
                    $("#disDiemKyII").attr("disabled", true);
                    $("#disDiemKyCN").css("color", "blue");
                    $("#disDiemKyCN").attr("disabled", true);
                    $("#" + Id_namhoc).removeClass("fa fa-plus").addClass(" fa fa-check");
                    $("#messTbDiem").text("Lưu thành công");
                    //xóa giá trị trong ô input sau khi đã thêm thành công
                    var allInputs = document.getElementsByTagName("input");
                    var results = [];
                    for (var x = 0; x < allInputs.length; x++)
                        allInputs[x].value = '';
                } else {
                    $("#messTbDiem").text("Lưu thất bại");
                }
            },
            error: function (err) {
                alert("Error: " + err.responseText);
            }
        });
    }
}

//function LuuDiem(nameHocKy, toan, ly, hoa, sinh, tin, van, su, dia, ngoaingu, congnghe, quocphong, theduc, tuchon, gdcd, diemTb) {
//    var diemky = {
//        TenKiHoc: nameHocKy,
//        Toan: $('#' + toan).val(),
//        Ly: $('#' + ly).val(),
//        Hoa: $('#' + hoa).val(),
//        Sinh: $('#' + sinh).val(),
//        Tin: $('#' + tin).val(),
//        Van: $('#' + van).val(),
//        Su: $('#' + su).val(),
//        Dia: $('#' + dia).val(),
//        NgoaiNgu: $('#' + ngoaingu).val(),
//        CongNghe: $('#' + congnghe).val(),
//        QuocPhongAnNinh: $('#' + quocphong).val(),
//        TheDuc: $('#' + theduc).val(),
//        TuChon: $('#' + tuchon).val(),
//        GiaoDucCongDan: $('#' + gdcd).val(),
//        DiemTrungBinh: $('#' + diemTb).val()
//    }

//    $.ajax({
//        type: "POST",
//        url: "/KyHoc/NhapDiem",
//        dataType: "json",
//        data: JSON.stringify(diemky),
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {
//            console.log(data);
//        },
//        error: function (err) {
//            alert("Error: " + err.responseText);
//        }
//    });
//}

//không sử dụng
//function DiemKyI() {
//    debugger;
//    var bool = validateDiem(0, "messTbDiemI");
//    if (bool == false)
//        return false;
//    else {
//        $("#messTbDiemI").text("Lưu thành công");
//        LuuDiem("Hkỳ I","ToanI","LyI","HoaI","SinhI","TinI","VanI","SuI","DiaI","NgoaiNguI","CongNgheI","QuocPhongAnNinhI","TheDucI","TuChonI","GiaoDucCongDanI","DiemTrungBinhI");
//        $("#disDiemKyI").css("color", "blue");
//        $("#disDiemKyI").attr("disabled", true);
//        k = k + 1;
//        if (k == 3) //xét trường hợp nhập đủ điểm của 3 kì trong 1 năm học
//            $("#" + Id_namhoc).removeClass("fa fa-plus").addClass(" fa fa-check"); 
//    }
//}
//function DiemKyII(){
//    debugger;
//    var bool = validateDiem(1,"messTbDiemII");
//    if (bool == false)
//        return false;
//    else {
//        $("#messTbDiemII").text("Lưu thành công");
//        LuuDiem("Hkỳ II","ToanII","LyII","HoaII","SinhII","TinII","VanII","SuII","DiaII","NgoaiNguII","CongNgheII","QuocPhongAnNinhII","TheDucII","TuChonII","GiaoDucCongDanII","DiemTrungBinhII");
//        $("#disDiemKyII").css("color", "blue");
//        $("#disDiemKyII").attr("disabled", true);
//        k = k + 1;
//        if (k == 3)
//            $("#" + Id_namhoc).removeClass("fa fa-plus").addClass(" fa fa-check");
//    }
//}
//function DiemKyCN() {
//    debugger;
//    var bool = validateDiem(2,"messTbDiemCN");
//    if (bool == false)
//        return false;
//    else {
//        $("#messTbDiemCN").text("Lưu thành công");
//        LuuDiem("CN","ToanCN","LyCN","HoaCN","SinhCN","TinCN","VanCN","SuCN","DiaCN","NgoaiNguCN","CongNgheCN","QuocPhongAnNinhCN","TheDucCN","TuChonCN","GiaoDucCongDanCN","DiemTrungBinhCN");
//        $("#disDiemKyCN").css("color", "blue");
//        $("#disDiemKyCN").attr("disabled", true);
//        k = k + 1;
//        if(k==3)
//            $("#" + Id_namhoc).removeClass("fa fa-plus").addClass(" fa fa-check");         
//    }
//}







//function DiemKyII() {
//    var diemkyII = {
//        TenKiHoc: 'Hkỳ II',
//        Toan: $('#ToanII').val(),
//        Ly: $('#LyII').val(),
//        Hoa: $('#HoaII').val(),
//        Sinh: $('#SinhII').val(),
//        Tin: $('#TinII').val(),
//        Van: $('#VanII').val(),
//        Su: $('#SuII').val(),
//        Dia: $('#DiaII').val(),
//        NgoaiNgu: $('#NgoaiNguII').val(),
//        CongNghe: $('#CongNgheII').val(),
//        QuocPhongAnNinh: $('#QuocPhongAnNinhII').val(),
//        TheDuc: $('#TheDucII').val(),
//        TuChon: $('#TuChonII').val(),
//        GiaoDucCongDan: $('#GiaoDucCongDanII').val(),
//        DiemTrungBinh: $('#DiemTrungBinhII').val()
//    }
//    $.ajax({
//        type: "POST",
//        url: "/KyHoc/NhapDiem",
//        dataType: "json",
//        data: JSON.stringify(diemkyII),
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {
//            console.log(data);
//        },
//        error: function (err) {
//            alert("Error: " + err.responseText);
//        }
//    });

//}
//function DiemKyCN() {
//    var diemCN = {
//        TenKiHoc: 'CN',
//        Toan: $('#ToanCN').val(),
//        Ly: $('#LyCN').val(),
//        Hoa: $('#HoaCN').val(),
//        Sinh: $('#SinhCN').val(),
//        Tin: $('#TinCN').val(),
//        Van: $('#VanCN').val(),
//        Su: $('#SuCN').val(),
//        Dia: $('#DiaCN').val(),
//        NgoaiNgu: $('#NgoaiNguCN').val(),
//        CongNghe: $('#CongNgheCN').val(),
//        QuocPhongAnNinh: $('#QuocPhongAnNinhCN').val(),
//        TheDuc: $('#TheDucCN').val(),
//        TuChon: $('#TuChonCN').val(),
//        GiaoDucCongDan: $('#GiaoDucCongDanCN').val(),
//        DiemTrungBinh: $('#DiemTrungBinhCN').val()
//    }
//    $.ajax({
//        type: "POST",
//        url: "/KyHoc/NhapDiem",
//        dataType: "json",
//        data: JSON.stringify(diemCN),
//        contentType: "application/json; charset=utf-8",
//        success: function (data) {
//            console.log(data);
//        },
//        error: function (err) {
//            alert("Error: " + err.responseText);
//        }
//    });
//}