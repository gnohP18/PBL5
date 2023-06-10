$(function() {

    const l = abp.localization.getResource('PBL5');

    const service = pBL5.timeSheets.timeSheet;

    updateModal = new abp.ModalManager(abp.appPath + 'Statistics/UpdateModal');
    statisticModal = new abp.ModalManager(abp.appPath + 'Statistics/DetailEmployeeStatisticModal');
    
    const dataTable = $('#EmployeeTable1').DataTable(
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
            ajax: abp.libs.datatables.createAjax(service.getTimeSheetByDate, GetValueSearch),
            columnDefs: [
            {
                title: l('TotalTimeDayWork'),
                data: "totalTimeDateWork",
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
                title: l('TimeSheet:IsAbsent'),
                data: "isAbsent",
            },
            {
                title: l('TimeSheet:Description'),
                data: "description",
            },
            {
                title: l('Action'),
                rowAction: {
                    items: [
                    {
                        text: 'Edit',
                        action: function (data) {
                            updateModal.open({ id: data.record.id })
                        },
                        displayNameHtml: true,
                    },
                    {
                        text: 'Delete',
                        confirmMessage: function (data) {
                            return l('TimeSheet:ConfirmedDelete',
                            luxon
                                .DateTime
                                .fromISO(data.record.dateCheckIn, {
                                    locale: abp.localization.currentCulture.name
                                }).toFormat('dd/MM/yyyy'), 
                            data.record.name);
                        },
                        action: function (data) {
                            service
                                .delete(data.record.id)
                                .then(function () {
                                    abp.notify.info(l('Common:SuccessfullyDeleted'));
                                    dataTable.ajax.reload();
                                });
                        },
                        displayNameHtml: true,
                    },
                ] 
                }
            },
        ]
    }));

    function GetValueSearch() {
        return {
            employeeId: $('#SearchViewModel_EmployeeCode').val(),
            dateSearch: ChangeFromUtcTimeToLocalTime($('#SearchViewModel_DateSearch')),
        }
    }

    $('#Search').click(function () {
        dataTable.ajax.reload();
    });

    $('#statistic').click(function () {
        console.log($('#SearchViewModel_DateSearch').val());
        console.log(ChangeFromUtcTimeToLocalTime($('#SearchViewModel_DateSearch')));
        statisticModal.open({ employeeId: $('#SearchViewModel_EmployeeCode').val(), month: $('#SearchViewModel_DateSearch').val()})
    });

    updateModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('Common:SuccessfullyUpdate'));
    });

    $('#empl_Name').val(dataName);
})