//format datetime
$(document).ready(function () {
    var now = new Date();
    var dateNow = now.getMonth() + 1 + "/" + now.getDate() + "/" + now.getFullYear();
    $("#dtpNgaySinhHB").val(dateNow);
    $("#dtpNgayLapHB").val(dateNow);
    $("#dtpNgaySinhHB").daterangepicker({
        singleDatePicker: true,
        calender_style: 'picker_1',
        locale: {
            format: "MM-DD-YYYY"
        },
    }, function (start, end, laber) {
        console.log(start.toISOString(), end.toISOString(), laber);
    });
    $("#dtpNgayLapHB").daterangepicker({

        singleDatePicker: true,
        calender_style: 'picker_1',
        locale: {
            format: "MM-DD-YYYY"
        },
    }, function (start, end, laber) {
        console.log(start.toISOString(), end.toISOString(), laber);
    });
});
var Id_namhoc = 0;
$(document).ready(function () {
    $("#filehb").change(function () {
        debugger;
        var data = new FormData();
        var files = $("#filehb").get(0).files;
        if (files.length > 0) {
            data.append("HelpSectionFile", files[0]);
        }
        $.ajax({
            url: "/XulyHocSinh/UpLoadFile",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (data) {
                if (data == "NO")
                    alert("Lỗi loại file lưu trữ");
            },
            error: function (er) {
                alert(er);
            }

        });
    });
});
function validateDiem(start, mess) {
    var inputs = document.getElementsByTagName('input');
    var i = start;
    while (i < inputs.length) {
        if (inputs[i].value == "" || inputs[i].value <= 0 || inputs[i].value > 10) {
            $("#" + mess).text("Điểm không được để rỗng. \nĐịnh dạng điểm phải 0 < x <=10");
            return false;
        }
        i = i + 1;
    }
    return true;
}
function validateDiemChung(mess, toan, ly, hoa, sinh, tin, van, su, dia, ngoaingu, congnghe, quocphong, theduc, tuchon, gdcd, diemTb) {
    if($('#' + toan).val() ==""||
             $('#' + ly).val()==""||$('#' + hoa).val()==""|| $('#' + sinh).val()==""||$('#' + tin).val()==""|| $('#' + van).val()==""||$('#' + su).val()==""||
            $('#' + dia).val()==""||
            $('#' + ngoaingu).val()==""||
            $('#' + congnghe).val()==""||
            $('#' + quocphong).val()==""||
            $('#' + theduc).val()==""||
            $('#' + gdcd).val()==""||
            $('#' + diemTb).val()=="")
    {
        $("#" + mess).text("Điểm không được để rỗng.");
        return false;
    }
    if ($('#' + toan).val() >10 ||
            $('#' + ly).val() > 10 || $('#' + hoa).val() > 10 || $('#' + sinh).val() > 10 || $('#' + tin).val() > 10 || $('#' + van).val() > 10 || $('#' + su).val() > 10 ||
           $('#' + dia).val() > 10 ||
           $('#' + ngoaingu).val() > 10 ||
           $('#' + congnghe).val() > 10 ||
           $('#' + quocphong).val() > 10 ||
           $('#' + theduc).val() > 10 ||
           $('#' + gdcd).val() > 10 ||
           $('#' + diemTb).val() > 10) {
        $("#" + mess).text("Định dạng điểm phải 0 < x <=10");
        return false;
    }
    if ($('#' + toan).val() <=0 ||
            $('#' + ly).val() <= 0 || $('#' + hoa).val() <= 0 || $('#' + sinh).val() <= 0 || $('#' + tin).val() <= 0 || $('#' + van).val() <= 0 || $('#' + su).val() <= 0 ||
           $('#' + dia).val() <= 0 ||
           $('#' + ngoaingu).val() <= 0 ||
           $('#' + congnghe).val() <= 0 ||
           $('#' + quocphong).val() <= 0 ||
           $('#' + theduc).val() <= 0 ||
           $('#' + gdcd).val() <= 0 ||
           $('#' + diemTb).val() <= 0) {
        $("#" + mess).text("Định dạng điểm phải 0 < x <=10");
        return false;
    }
    return true;
}
function SaveHocBa() {
    if ($("#HoTenHB").val() == "" || $("#dtpNgaySinhHB").val() == "" || $("#NoiSinhHB").val() == "" || $('[name="GioiTinh"]:radio:checked').val() == "" ||
        $("#DanTocHB").val() == "" || $("#NoiSongHienTaiHB").val() == "" || $("#HoTenBoHB").val() == "" || $("#NgheNghiepBoHB").val() == "" ||
         $("#HoTenMeHB").val() == "" || $("#NgheNghiepMeHB").val() == "" || $("#dtpNgayLapHB").val() == "") {
        $("#messThemHB").text("*****Thông tin không được để trống");
        return false;
    }
    var hocba = {
        HoTen: $("#HoTenHB").val(),
        NgaySinh: $("#dtpNgaySinhHB").val(),
        NoiSinh: $("#NoiSinhHB").val(),
        GioiTinh: $('[name="GioiTinh"]:radio:checked').val(),
        DanToc: $("#DanTocHB").val(),
        NoiSongHienTai: $("#NoiSongHienTaiHB").val(),
        HoTenBo: $("#HoTenBoHB").val(),
        NgheNghiepBo: $("#NgheNghiepBoHB").val(),
        HoTenMe: $("#HoTenMeHB").val(),
        NgheNghiepMe: $("#NgheNghiepMeHB").val(),
        NguoiGiamHo: $("#NguoiGiamHoHB").val(),
        NgheNghiepNGH: $("#NgheNghiepNGHHB").val(),
        NgayLap: $("#dtpNgayLapHB").val()

    };
    $.ajax({
        type: "POST",
        url: "/HocBa/Themmoi",
        dataType: "json",
        data: JSON.stringify(hocba),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            if (data == "YES") {
                $("#ClassHB").empty();
                $("#formNH").css("display", "block"); //để hiện thị form nhập năm học
                $("#formLoadNH").css("display", "block");
                document.getElementById("ThemmoiNamHoc").reset();
            } else {
                alert("Thêm mới thất bại !");
            }
        },
        error: function (err) {
            alert("Error: " + err.responseText);
        }
    });
}
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
                rows += '<i id="' + data.id + '"  class="fa fa-plus"></i>';
                rows += '</a>';
                rows += '<a data-toggle=tooltip data-placement=bottom  title="Sửa" href="/NamHoc/Suanamhoc/'+data.id+'">';
                rows += '<i  class="fa fa-edit"></i>';
                rows += '</a>';
                rows += '<a data-toggle=tooltip data-placement=bottom  title="Xoa" href="/NamHoc/Xoanamhoc/' + data.id + '">';
                rows += '<i  class="fa fa-trash-o"></i>';
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
function LoadFormNhapDiem(id_namhoc) {
    Id_namhoc = id_namhoc;
    $.ajax({
        type: "GET",
        url: "/NamHoc/LoadFormNhapDiem",
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
