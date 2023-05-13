# Capa de Acceso a datos para iParking

En este proyecto estarán todo el mapeo de tabla de la base de datos de iParking usando EntityFrameworkCore. Todas las clases y DbContext son autogenerados desde la base de datos y NO deben ser cambiados. Este enfoque es llamado: **Database First** o **Reverse Engineer**

https://docs.microsoft.com/en-us/ef/core/managing-schemas/scaffolding?tabs=dotnet-core-cli
https://entityframeworkcore.com/approach-database-first
https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx

Para una mejor organización y efiencia en el uso de EFCore es necesario que cada funcionalidad tenga su propio DbContext de manera que se puedan agrupar las tablas por el contexto de funcionalidad, y solo se accedan las tablas necesarias.

El commando para crear un DbContext es: 

~~~
`dotnet ef dbcontext scaffold "<connectionString>" Microsoft.EntityFrameworkCore.SqlServer -o Data -d -t <TableName> -c <DbContextName> -f`
~~~

1. Sustituir <connectionString> con una connectionString válida para SqlServer. 
2. Sustituir <TableName> por el nombre de la tabla que se desea incluir en el DbContext. Si se desea incluir varias tablas es necesario repetir la opción -t <TableName>. Si esta opción no está presente el comando creará un DbContext con todas las tablas de la Bd. (evitar*)
3. Sustituir <DbContextName> con el nombre del DbContext que se quiere crear. 
4. La opción -f fuerza a sobreescribir los ficheros existentes. Es necesario usar esta opción con precaución: si se escoje un nombre de DbContext existente este se sobreescribirá completamente. 
5. Al ejecutar el comando de manera satisfactoria es necesario revisar el DbContext creado para eliminar información sensible de la connectionString. 
6. Al ejecutar el comando siempre es necesario una revisión de los cambios a través de Git para estar asegurarse que todo está como se espera antes de confirmar.

https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx

https://docs.microsoft.com/en-us/ef/core/