(*** hide ***)
#I "../../files/sqlite"
(*** hide ***)
#I "../../../bin"
(*** hide ***)
#r @"../../../bin/FSharp.Data.SqlProvider.dll"

(*** hide ***)
[<Literal>]
let connectionString = "Data Source=" + __SOURCE_DIRECTORY__ + @"/../../../tests/SqlProvider.Tests/scripts/northwindEF.db;Version=3"

(*** hide ***)
[<Literal>]
let resolutionPath = __SOURCE_DIRECTORY__ + @"/../../files/sqlite"

(**
## Adding a Mapper using dataContext to use generated types from db

This mapper will get sure that you always sync your types with types you receive from your db.

First add an Domain Model

*)

type Employee = {
    EmployeeId : int64
    FirstName : string
    LastName : string
    HireDate : DateTime
}

(**
Then you can create the mapper using dataContext to use generated types from db
*)

let mapEmployee (dbRecord:sql.dataContext.``main.EmployeesEntity``) : Employee =
    { EmployeeId = dbRecord.EmployeeId
      FirstName = dbRecord.FirstName
      LastName = dbRecord.LastName
      HireDate = dbRecord.HireDate }

(**
SqlProvider also has a `.MapTo<'T>` convenience  method:
*)

open System
open FSharp.Data.Sql

type sql = SqlDataProvider<Common.DatabaseProviderTypes.SQLITE,
                           connectionString,
                           ResolutionPath = resolutionPath,
                           CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL>

let ctx = sql.GetDataContext()

let orders = ctx.Main.Orders
let employees = ctx.Main.Employees

type Employee2 = {
    FirstName:string
    LastName:string
    }

let qry = query { for row in employees do
                  select row} |> Seq.map (fun x -> x.MapTo<Employee2>())



