$(function () {
    const l = abp.localization.getResource('PBL5');

    const service = pBL5.employees.employee;
    const createModal = new abp.ModalManager(abp.appPath + 'Employees/CreateModal');
    const updateModal = new abp.ModalManager(abp.appPath + 'Employees/UpdateModal');
    const changePasswordModal = new abp.ModalManager(abp.appPath + 'Employees/ChangePasswordModal');

    const dataTable = $('#EmployeeTable').DataTable(
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
                title: l('Common:GenderName'),
                data: "genderName",
                render: function (data) {
                    return l(data)
                },
            },
            {
                title: l('Employee:DateOfBirth'),
                data: "dateOfBirth",
                render: function (data) {
                    return luxon
                        .DateTime
                        .fromISO(data, {
                            locale: abp.localization.currentCulture.name
                        }).toFormat('dd/MM/yyyy');
                },
            },
            {
                title: l('Employee:EmployeeCode'),
                data: "employeeCode",
            },
            {
                title: l('Employee:Email'),
                data: "email",
            },
            {
                title: l('Employee:Phone'),
                data: "phone",
            },
            {
                title: l('Edit'),
                rowAction: {
                    items: [{
                        text: '<i class="fa fa-pencil-square-o"></i>',
                        //visible: abp.auth.isGranted('PBL5Permissions.Employee.Update'),
                        action: function (data) {
                            updateModal.open({ id: data.record.id })
                        },
                        displayNameHtml: true,
                    },
                ] 
                }
            },
            {
                title: l('Delete'),
                rowAction: {
                    items: [{
                        text: '<i class="fa fa-trash-o danger"></i>',
                        //visible: abp.auth.isGranted('PBL5Permissions.Employee.Delete'),
                        confirmMessage: function (data) {
                            return l('Common:DeletionConfirmationMessage', data.record.name);
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
            {
                title: l('Employee:ChangePassword'),
                rowAction: {
                    items: [{
                        text: '<i class="fa fa-lock"></i>',
                        //visible: abp.auth.isGranted('PBL5Permissions.Employee.Delete'),
                        action: function (data) {
                            changePasswordModal.open({ id: data.record.id })
                        },
                        displayNameHtml: true,
                    },
                ] 
                }
            }
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

    $(window).resize(function () {
        resizeWindowHandler(dataTable)
    });

    $('#NewEmployee').click(function (e) {
        e.preventDefault();
        createModal.open();
    });

    createModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('Common:SuccessfullyCreated'));
    });

    updateModal.onResult(function () {
        dataTable.ajax.reload();
        abp.notify.success(l('Common:SuccessfullyUpdate'));
    });
})