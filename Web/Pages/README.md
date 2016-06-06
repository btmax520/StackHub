# ynrcjt
云南镕诚集团众包平台项目
众包招聘求职

# 唯一序列数字
标识在系统中独一无二的一个数据字

一个序列号 匹配唯一一个object

sequence.create();
sequence.create(1);
Get() return one

Get(5) return some

# 元数据
类似sap的 Domain、Data Elements
Data Elements
元素
用于聚合特定的数据 数据字段 表字段
如：系统中可以统一规范的不可再分的类型，如物料名称、颜色、重量单位（顿）

如果有很多表都要用颜色，都可以引用颜色这个已经定义好的元素

必须系统中设置某一数据类型 如：衣服颜色 车身颜色 RGB色 16进制色


Elements Specification
规格
元素的特性如：数据类型、数据范围、可选数值
定义字段类型（数字、字符等），字段长度，输出长度，小数点位数（如果是数字），可选值等信息
如颜色 可以是数字形式 也可以是枚举 还可以限制颜色的范围等

validator 验证器
用于验证数据是否符合要求

calculator 计算器
数据的计算方式 比如：总价=单价x数量


目的
在系统中可以知道某个字段 某个数据元素 某个元素规格都在哪些地方被引用了。
评估系统的相互引用关系 耦合度
统一规范
# 表单系统




baseobject bo
id value code name

lable name id value code type ref object

type 基于 json

String
Number
Boolean
Array
Object
Null

输入控件
引用空间


# 基础数据
1、发布
服务类型
服务名称
服务描述
服务费用
服务周期
技能要求
人员要求
附件

两个要素 需求分类，需求描述
tag category catlog

lable input select

2、接单