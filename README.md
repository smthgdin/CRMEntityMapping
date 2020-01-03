提供Fluent和Convention两种方式来实现自动转换。
支持Dynamics CRM Entity和BizEntity/DTO的双向转换。

1.Fluent 类似 EF Fluent，基于配置，通过配置实现领域模型和Entity的映射，支持双向映射。
优点：非常灵活，扩展性、维护性好；少写很多赋值语句。
缺点：就是要写映射配置文件，如果代码中转换用得少，则不能体现有点。
见基础设置项目下【ORM】-->【MappingFiles】下的文件。目前还缺少对实体引用的支持。

2.Convention 基于约定，约定领域模型的属性名和Entity属性名一致（不区分大小写）。
优点：不需要配很多映射配置文件。
缺点：不够灵活，如果双方命名和类型不同时没法处理。
自定义特性类见【EntityMappingAttribute】文件夹。
