export const enum eBasePolicyNames {
  Identity = 'Base.Employee|| Base.Customer  || Base.Vendor ',

  Employees = 'Base.Employee.Create || Base.Employee.Update || Base.Employee.List || Base.Employee.Delete || Base.Employee.Undo',
  EmployeesNew = 'Base.Employee.Create || Base.Employee.Update',
  EmployeeList = 'Base.Employee.List || Base.Employee.Delete || Base.Employee.Undo',

  Customers = 'Base.Customer.Create || Base.Customer.Update || Base.Customer.List || Base.Customer.Delete || Base.Customer.Undo',
  CustomerNew = 'Base.Customer.Create || Base.Customer.Update',
  CustomerList = 'Base.Customer.List || Base.Customer.Delete || Base.Customer.Undo',
  CustomerVehicleList = 'Base.Customer.List || Base.Customer.Delete || Base.Customer.Undo',

  Vendor = 'Base.Vendor.Create || Base.Vendor.Update || Base.Vendor.List || Base.Vendor.Delete || Base.Vendor.Undo',
  VendorNew = 'Base.Vendor.Create || Base.Vendor.Update',
  VendorList = 'Base.Vendor.List || Base.Vendor.Delete || Base.Vendor.Undo',



  IdentityManagement = 'Base.Company.Update|| Base.VehicleType.Update  || Base.Address.Update ',
  Company = 'Base.Company.Update || Base.Company.List',
  Address = 'Base.Address.Update || Base.Address.List',
  VehicleType = 'Base.VehicleType.Create || Base.VehicleType.Update || Base.VehicleType.List || Base.VehicleType.Delete || Base.VehicleType.Undo',

  DocNumbers = 'Base.DocNumber || Base.DocNumber.Update || Base.DocNumber.List',
}