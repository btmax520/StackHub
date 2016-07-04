var mainCtrl = {
    server : "http://localhost:54007/api/Values",
    menuIndex : -1,
    menuClick: function (index, row) {
        if (index != this.menuIndex) {
            //$('#leftContainer').empty().append(
            //    '<div id="primaryGrid" title="数据视图" class="easyui-datagrid" style="width:100%;"></div>'
            //);
            switch (row.code) {
                case "schema":
                    schemaCtrl.show('list');
                    break;
                case "category":
                    break;
                case "tag":
                    break;
            }
            menuIndex = index;
        }
    },
    actionHandler: {
        save: function () {
            var changes = $('#primaryGrid').datagrid('getChanges');
            var postData = {
                type: "schema",
                rows: changes
            }
            var url = "http://localhost:54007/api/Values/";
            data = postData;
            $.post(
                url,
                data,
                function (data, status, xhr) {
                    for (var i = 0; i < data.rows.length; i++) {
                        index = $('#primaryGrid').datagrid('getRowIndex', data.rows[i]);
                        if (index >= 0) {
                            $('#primaryGrid').datagrid('updateRow', { index:index ,row : data.rows[i]});
                        }
                        else {
                            $('#primaryGrid').datagrid('appendRow', data.rows[i]);
                        }
                        
                        $('#primaryGrid').datagrid('acceptChanges');
                    }
                },
                "json"
            );
        },
        find: function () { alert('back') },
        detail: function () {
            alert(schemaCtrl.context.rowIndex);
        },
        add:function(){
            $('#primaryGrid').datagrid('appendRow', {});
        },
        del:function(){
            var rows = $('#primaryGrid').datagrid('getChecked');
            $.messager.confirm('确认删除', '是否删除选中行数据?', function (r) {
                if (r) {
                    for (i = 0; i < rows.length; i++) {
                        var index = $('#primaryGrid').datagrid('getRowIndex', rows[i]);
                        $('#primaryGrid').datagrid('deleteRow', index);
                    }
                }
            });
        }
    }
}

var schemaCtrl = {
    currentPage: 1,
    context: {
        rowIndex : -1
    },
    listCtrl :{
        columns: [[
            { field: "id", title: "ID", width: 120, editor: " text" },
            { field: "namespace", title: "命名空间", width: 100, editor: "text" },
            { field: "code", title: "编码", width: 100, editor: "text" },
            { field: "name", title: "名称", width: 200, editor: "text" },
            { field: "descriptin", title: "描述", width: 400, editor: "text" },
        ]],
        actions: [
            { text: '保存', handler: mainCtrl.actionHandler.save },
            '-',
            { 
                text: '新增', 
                handler: function () {
                    var defaultRow = {};
                    for (i = 0; i < schemaCtrl.listCtrl.columns.length; i++)
                    {
                        defaultRow[schemaCtrl.listCtrl.columns[0][i].field] = "";
                    }
                    $('#primaryGrid').datagrid('appendRow', defaultRow);
                } 
            },
            { text: '删除', onClick: function () { alert('del') } },
            {
                text: '详细',
                id: "1001000000000001",
                onClick: function () {
                    //alert(this.id);
                    schemaCtrl.show('detail',this.id);
                }
            },
            '-',
            { text: '查找', handler: mainCtrl.actionHandler.find },
        ]
    },
    columns: [[
        { field: "id",title: "ID",width :40, editor :" text" },
        { field: "code", title: "字段编码",width :100, editor: "text" },
        { field: "name", title: "字段名称", width: 100, editor: "text" },
        { field: "type", title: "字段类型", width: 100, editor: "text" },
        { field: "lenght", title: "长度", width: 40, editor: 'numberbox' },
        { field: "precision", title: "精度", width: 40, editor: "numberbox" },
    ]],
    show: function (type, id) {
        $('#leftContainer').empty().append(
            '<div id="primaryGrid" title="数据视图" class="easyui-datagrid" style="width:100%;"></div>'
        );
        if (type == 'list') {
            $('#primaryGrid').datagrid({
                columns: this.listCtrl.columns,
                title: '数据架构',
                loadMsg:'正在拼命加载数据……',
                idField: "id",
                data: this.load(),
                singleSelect: true,
                rownumbers:true,
                pagination: true,
                pageNumber:this.currentPage,
                pageSize:20,
                height:640,
                toolbar: this.listCtrl.actions,
                onClickRow: function (index) {
                    schemaCtrl.context.rowIndex = index;
                },
                onAfterEdit: function (index, row, changes) {
                    alert(JSON.stringify(row));
                    alert(JSON.stringify(changes));
                },
                onRowContextMenu: function (e, index, row) {
                    alert(JSON.stringify(row));
                    e.preventDefault();
                }
            }).datagrid('enableCellEditing').datagrid("loading");;
        }
        else{
            $('#primaryGrid').datagrid({
                columns: this.columns,
                title: '数据字段',
                //idField:"itemid",
                singleSelect: true,
                //toolbar: getUIAction(),
                //onClickRow: function (index) {
                //    //$('#dg').datagrid('selectRow', index);
                //},

            }).datagrid('enableCellEditing');
        }
    },
    load: function () {
        var url = "http://localhost:54007/api/Values/";
        queryParams = {
            type: "schema",
            pageIndex: 1,
            pageSieze:20
        }
        $.get(
            url,
            { q: JSON.stringify(queryParams) },
            function (data, status, xhr) {
                $('#primaryGrid').datagrid('loadData', data.rows);
            },
            "json"
        );
    }
}