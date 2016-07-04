var defaultConfig = {
    toolbar: [
        {
            code: "new",
            cmd: "&new",
            text:"新增"
        },
        {
            code: "save",
            cmd: "&save",
            text:"保存"
        },
        {
            code: "find",
            cmd: "&find",
            text: "查找"
        },
        {
            code: "active",
            cmd: "&active",
            text: "激活"
        }
    ]
}
var metedata = {
    fields : [
        {
            code: "字段编码",
            name: "字段名称",
            id: "字段数值",
        }
    ]
}

var schemaDefine ={
    id : {
        code : "ID",
        name : "ID",
        ref : "string",
        type : "string",
        lenght:20
    },
    code: {
        code: "code",
        name: "字段编码",
        ref: "string",
        type: "string",
        lenght: 20
    },
    name: {
        code: "name",
        name: "字段名称",
        ref: "string",
        type: "string",
        lenght: 20
    },
    group: {
        code: "group",
        name: "字段分组",
        ref: "string",
        type: "string",
        lenght: 20
    },
    type:{
        code : "type",
        name : "数据类型",
        ref : "system:base:type",
        type : "string",
        lenght:20 
    }
}

var baseSchema = {
    key: "system:schema",
    descripttion: "系统基础数据构架",
    fields: {
        id:
        {
            code: "ID",
            name: "ID",
            ref: "string",
            type: "string",
            lenght: 20
        },
        code: {
            code: "code",
            name: "字段编码",
            ref: "string",
            type: "string",
            lenght: 20
        },
        name: {
            code: "name",
            name: "字段名称",
            ref: "string",
            type: "string",
            lenght: 20
        },
        group: {
            code: "group",
            name: "字段分组",
            ref: "string",
            type: "string",
            lenght: 20
        },
        type: {
            code: "type",
            name: "数据类型",
            ref: "system:base:type",
            type: "string",
            lenght: 20
        },
        lenght: {
            code: "lenght",
            name: "字段长度",
            ref: "number",
            type: "number",
            lenght: 4
        },
        precision: {
            code: "precision",
            name: "小数位数",
            ref: "number",
            type: "number",
            precision:0,
            lenght: 4
        },
    }
}

var metadataSchema = {
    key:"metadata:schema",
    fields: [
        { code: "code", name: "字段编码", id: "metadata:schema:code" },
        { code: "name", name: "字段名称", id: "metadata:schema:name" },
        { code: "group", name: "字段分组", id: "metadata:schema:group" },
        { code: "ref", name: "字段引用", id: "metadata:schema:ref" },
        { code: "type", name: "字段类型", id: "metadata:schema:type" },
        { code: "lenght", name: "字段长度", id: "metadata:schema:lenght" },
        { code: "decimal", name: "字段小数", id: "metadata:schema:decimal" },
        { code: "descript", name: "字段描述", id: "metadata:schema:descript" },
        { code: "tip", name: "字段帮助", id: "metadata:schema:tip" },
        { code: "text", name: "字段文本", id: "metadata:schema:text" },
    ]
}

var treeGrid = {
    "total": 7, "rows": [
        { "id": 1, "name": "All Tasks", "begin": "3/4/2010", "end": "3/20/2010", "progress": 60, "iconCls": "icon-ok" },
        { "id": 2, "name": "Designing", "begin": "3/4/2010", "end": "3/10/2010", "progress": 100, "_parentId": 1, "state": "closed" },
        { "id": 21, "name": "Database", "persons": 2, "begin": "3/4/2010", "end": "3/6/2010", "progress": 100, "_parentId": 2 },
        { "id": 22, "name": "UML", "persons": 1, "begin": "3/7/2010", "end": "3/8/2010", "progress": 100, "_parentId": 2 },
        { "id": 23, "name": "Export Document", "persons": 1, "begin": "3/9/2010", "end": "3/10/2010", "progress": 100, "_parentId": 2 },
        { "id": 3, "name": "Coding", "persons": 2, "begin": "3/11/2010", "end": "3/18/2010", "progress": 80 },
        { "id": 4, "name": "Testing", "persons": 1, "begin": "3/19/2010", "end": "3/20/2010", "progress": 20 }
    ], "footer": [
        { "name": "Total Persons:", "persons": 7, "iconCls": "icon-sum" }
    ]
}


var createToolBar = function () {
    
}

var emptyPostData = {
    context: {},
    query: {

    },
    rows: [
        { type: "system:data", id: "1234567890", name: "12123", curd: "insert" },
        { type: "system:data", id: "1234567890", name: "12123", curd: "update" },
        { type: "system:data", id: "1234567890", name: "12123", curd: "delete" },
    ]
}

var saveData = function (postData,callback) {
    var url = "http://localhost:54007/api/Values/";
    alert(JSON.stringify(postData));
    $.post(url, postData, callback, "json");
}