using System;
using System.Collections.Generic;
using System.Linq;

namespace Generics.Tables
{
    public class Table<Row, Column, Value>
    {
        public Dictionary<Tuple<Row, Column>, Value> TableValues;
        public List<Row> Rows = new List<Row>();
        public List<Column> Columns = new List<Column>();
        public OpenClass Open;
        public ExistedClass Existed;

        public Table()
        {
            this.TableValues = new Dictionary<Tuple<Row, Column>, Value>();
            this.Open = new OpenClass(this);
            this.Existed = new ExistedClass(this);
        }

        public void AddRow(Row row)
        {
            if (!Rows.Contains(row))
            {
                Rows.Add(row);
            }
        }

        public void AddColumn(Column column)
        {
            if (!Columns.Contains(column))
            {
                Columns.Add(column);
            }
        }

        public class ExistedClass
        {
            Table<Row, Column, Value> ExistTable;

            public ExistedClass(Table<Row, Column, Value> table)
            {
                this.ExistTable = table;
            }

            public Value this[Row row, Column column]
            {

                get
                {
                    Tuple<Row, Column> key = Tuple.Create(row, column);
                    if (!ExistTable.Rows.Contains(row))
                    {
                        throw new ArgumentException();
                    }

                    if (!ExistTable.Columns.Contains(column))
                    {
                        throw new ArgumentException();
                    }

                    if (ExistTable.TableValues.ContainsKey(key))
                    {
                        return ExistTable.TableValues[key];
                    }
                    return default(Value);
                }

                set
                {
                    Tuple<Row, Column> key = Tuple.Create(row, column);
                    if (!ExistTable.Rows.Contains(row))
                    {
                        throw new ArgumentException();
                    }

                    if (!ExistTable.Columns.Contains(column))
                    {
                        throw new ArgumentException();
                    }
                    ExistTable.TableValues[key] = value;
                }
            }
        }

        public class OpenClass
        {
            Table<Row, Column, Value> table;

            public OpenClass(Table<Row, Column, Value> table)
            {
                this.table = table;
            }

            public Value this[Row row, Column column]
            {
                get
                {
                    Tuple<Row, Column> key = Tuple.Create(row, column);
                    if (table.Rows.Contains(row) && table.Columns.Contains(column))
                    {
                        if (table.TableValues.ContainsKey(key))
                        {
                            return table.TableValues[key];
                        }
                    }
                    return default(Value);
                }

                set
                {
                    var key = Tuple.Create(row, column);
                    if (!table.Rows.Contains(row))
                    {
                        table.Rows.Add(row);
                    }

                    if (!table.Columns.Contains(column))
                    {
                        table.Columns.Add(column);
                    }
                    table.TableValues[key] = value;
                }
            }
        }
    }
}