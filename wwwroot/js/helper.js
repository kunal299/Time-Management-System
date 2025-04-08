var helper = {};

helper.DateForamt2 = function (inDate) {
    var ret = dayjs(inDate).format('DD-MMM-YYYY hh:mm a');
    return ret;

}

helper.DateForamt3 = function (inDate) {
    var ret = dayjs(inDate).format('YYYY-MM-DD');
    return ret;

}

helper.DateForamt1 = function (inDate) {
    var ret = dayjs(inDate).format('DD-MMM-YYYY');
    return ret;

}

helper.SuccessToast = function (msg) {
    $.toast({
        displayTime: 5000,
        message: msg,
        class: 'success',
        showProgress: 'bottom'
    })
}

helper.ErrorToast = function (msg) {
    $.toast({
        displayTime: 5000,
        message: msg,
        class: 'error',
        showProgress: 'bottom'
    })
}

helper.toTable = function (json) {
    var tbl = '<table class="ui celled padded table">';
    tbl += '<thead><tr>';
    _.each(_.keys(json[0]), function (k) {
        tbl += '<th>' + k + '</th>';
    });
    tbl += '</tr></thead>';

    _.each(json, function (row) {
        tbl += '<tr>';
        _.each(_.keys(json[0]), function (k) {
            tbl += '<td>' + row[k] + '</td>';
        });
        tbl += '</tr>';
    });
    tbl += '</table>';
    return tbl;
};

helper.ShowModal = function (content) {
    $("#mdlContent").html(content);
    $('#mdlBasic')
        .modal('show')
        ;
}

helper.ShowAuditModal = function (Id, page) {
    var url = window.location.origin + "/Widgets/Admin/VersionHistoryModal?Id=" + Id + "&t=" + page;
    $.ajax({
        url: url,
        success: function (result) {
            $("#mdlContentFullScreen").html(result);
            $('#mdlBasicFullScreen').modal('show');
        },
        error: function (err) {
            console.error('Error: ', err);
        }
    });

}

helper.RecruiterProfilePath = function (recruiter) {

    var profilepath = '/images/Profile/user.png';
    if (_.size(_.where(objData.Recruiters, { ShortName: recruiter })) > 0) {
        profilepath = _.first(_.where(objData.Recruiters, { ShortName: recruiter })).ProfilePath;
    }
    return profilepath;
}
helper.DisplayRecruiter = function (recruiter, length) {
    profilepath = helper.RecruiterProfilePath(recruiter);

    var str = '';
    length != 0 ? str += `<a class="ui teal large label" style="width:${length}px" onclick=Javascript:helper.ShowRecruiterModal("` + recruiter + `")>` : str += `<a class="ui teal large label">`;
    str += '<img class="ui right spaced avatar image" src="' + profilepath + '">' + recruiter + '</a>';
    return str;

}

helper.ShowRecruiterModal = function (recruiter) {

    var url = window.location.origin + "/recruitermodalpage?r=" + recruiter;
    $.ajax({
        url: url,
        success: function (result) {
            $("#mdlContentFullScreen").html(result);
            $('#mdlBasicFullScreen')
                .modal('show')
                ;

        }
    }
    );


}