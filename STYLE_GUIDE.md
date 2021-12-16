# Moondown style guide
All files in the repository should comply with the
style guide specified below. Any files that do not comply
should be edited.

## 1) Naming
All files should use C# naming conventions,
this includes:
* PascalCase for namespaces, methods, properties,
classes, structs and interfaces
* Interfaces should start with an I
* camelCase for fields and parameters
* There is no need to start private fields with
an `_`
* **Do not** begin static fields with `s_` and members
with `m_`
* One class/struct/enum/interface per file
* Filename should match class/struct/enum/interface name

## 2) Content 
Each file must begin with the following statement:
```
/*
    Copyright (C) 2021 Moondown Project

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
```
