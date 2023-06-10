$(function () {
    const l = abp.localization.getResource('PBL5');

    const service = pBL5.timeSheets.timeSheet;
    const createModal = new abp.ModalManager(abp.appPath + 'TimeSheets/CreateModal');
    const showImageModal = new abp.ModalManager(abp.appPath + 'TimeSheets/IdentificationImageModal');
    let idCheckIn = 0;
    $('#SearchViewModel_DayWork').datepicker("setDate", new Date(dayWork));

    const dataTable = $('#TimeSheetTable').DataTable(
        abp.libs.datatables.normalizeConfiguration({
            processing: true,
            responsive: true,
            serverSide: true,
            paging: true,
            searching: false,
            autoWidth: true,
            fixedColumns: true,
            fixedHeader: true,
            bLengthChange: true,
            scrollCollapse: true,
            ajax: abp.libs.datatables.createAjax(service.searchList, GetValueSearch),
            columnDefs: [{
                title: l('Employee:Name'),
                data: "employeeName",
            },
            {
                title: l('Employee:EmployeeCode'),
                data: "employeeCode",
            },
            {
                title: l('TimeSheet:CheckInTime'),
                data: "checkInTime",
                render: function (data) {
                    return (data != null) ? (new Date(data)).toLocaleString() : '';
                }
            },
            {
                title: l('TimeSheet:CheckOutTime'),
                data: "checkOutTime",
                render: function (data) {
                    return (data != null) ? (new Date(data)).toLocaleString() : '';
                }
            },
            {
                title: l('TimeSheet:DateCheckIn'),
                data: "dateCheckIn",
                render: function (data) {
                    return luxon
                        .DateTime
                        .fromISO(data, {
                            locale: abp.localization.currentCulture.name
                        }).toFormat('dd/MM/yyyy');
                },
            },
            {
                title: l('TimeSheet:WorkStatus'),
                data: "workStatus",
            },
            {
                title: l('TimeSheet:Description'),
                data: "description",
            },
            {
                title: l('Image check-in'),
                rowAction: {
                    items: [{
                        text: '<i class="fa fa-pencil-square-o"></i>',
                        action: function (data) {
                            idCheckIn = 1;
                            showImageModal.open({ id: data.record.id, type: true })
                        },
                        displayNameHtml: true,
                    },
                ] 
                }
            },
            {
                title: l('Image check-out'),
                rowAction: {
                    items: [{
                        text: '<i class="fa fa-pencil-square-o"></i>',
                        action: function (data) {
                            idCheckIn = 0;
                            showImageModal.open({ id: data.record.id, type: false })
                        },
                        displayNameHtml: true,
                    },
                ] 
                }
            },
            ]
        })
    );

    function GetValueSearch() {
        return {
            keySearch: $('#SearchViewModel_KeySearch').val(),
            dayWork: ChangeFromUtcTimeToLocalTime($('#SearchViewModel_DayWork')),
        }
    }

    $('#Search').click(function () {
        dataTable.ajax.reload();
    });

    $(window).resize(function () {
        resizeWindowHandler(dataTable)
    });

    $('#NewTimeSheet').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    createModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('Common:SuccessfullyCreated'));
    });

    $(document).on('show.bs.modal', '#timeSheetForm', function (e) {
        service.getPathFileFromTimeSheet($('#ViewModel_Id').val(), $("#IsCheckIn").val())
            .then(function (result) {
                if(result)
                {
                    let imageUrl = result.substring(result.indexOf('UploadFile'));
                    console.log(imageUrl);
                    $('#imgCheckIn').attr('src', "/" + imageUrl);
                }
            });
    })

    setInterval(function() {
        dataTable.ajax.reload(null, false);
      }, 60000);
})