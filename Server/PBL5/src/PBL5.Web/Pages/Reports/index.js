$(function () {
    const l = abp.localization.getResource('PBL5');

    const service = pBL5.reports.report;
    solveReportModal = new abp.ModalManager(abp.appPath + 'Reports/SolveReportModal');

    const dataTable = $('#ReportTable').DataTable(
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
            ordering: [
                [1, "asc"]
            ],
            ajax: abp.libs.datatables.createAjax(service.searchList, GetValueSearch),
            columnDefs: [{
                title: l('Employee:Name'),
                data: "name",
            },
            {
                title: l('Employee:EmployeeCode'),
                data: "employeeCode",
            },
            {
                title: l('Report:ReportDate'),
                data: "reportDate",
                render: function (data) {
                    return luxon
                        .DateTime
                        .fromISO(data, {
                            locale: abp.localization.currentCulture.name
                        }).toFormat('dd/MM/yyyy');
                },
            },
            {
                title: l('Report:ReportStatus'),
                data: "reportStatus",
                render: function (data) {
                    if (data){
                        if (data == 'Accept') return `<button type="button" class="btn btn-outline-success btn-sm">Accept</button>`
                        if (data == 'Pending') return `<button type="button" class="btn btn-outline-warning btn-sm">Pending</button>`
                        if (data == 'Deny') return `<button type="button" class="btn btn-outline-danger btn-sm">Deny</button>`
                    }
                    else
                    {
                        return `<button type="button" class="btn btn-outline-secondary btn-sm">No data</button>`
                    }
                }
            },
            {
                title: l('Report:Content'),
                data: "content",
            },
            {
                title: "Type of report",
                data: "typeOfReport",
            },
            {
                title: l('Edit'),
                rowAction: {
                    items: [{
                        text: '<i class="fa fa-pencil-square-o"></i>',
                        action: function (data) {
                            solveReportModal.open({ id: data.record.id })
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
        }
    }

    $('#Search').click(function () {
        dataTable.ajax.reload();
    });

    solveReportModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('Common:SuccessfullyUpdate'));
    });

    $(window).resize(function () {
        resizeWindowHandler(dataTable)
    });

})