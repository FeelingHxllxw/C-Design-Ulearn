using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incapsulation.EnterpriseTask
{
	public class Enterprise
	{
		private Guid guid;
		private string name;
		private string inn;
		private DateTime establishDate;

		public Guid Guid { get; }
		public string Name { get; set; }
		public string Inn
		{
			get
			{
				return inn;
			}
			set
			{
				if (inn.Length != 10 || !inn.All(z => char.IsDigit(z)))
					throw new ArgumentException();
				inn = value;
			}
		}
		public DateTime EstablishDate { get; set; }
		public TimeSpan ActiveTimeSpan
		{
			get
			{
				return DateTime.Now - EstablishDate;
			}
		}

		public Guid GetGuid() { return guid; }
		public Enterprise(Guid guid) { this.guid = guid; }
		public string GetName() { return name; }
		public void SetName(string name) { this.name = name; }
		public string GetINN() { return inn; }
		public void SetINN(string inn)
		{
			if (inn.Length != 10 || !inn.All(z => char.IsDigit(z)))
				throw new ArgumentException();
			this.inn = inn;
		}

		public DateTime GetEstablishDate() { return establishDate; }
		public void SetEstablishDate(DateTime establishDate) { this.establishDate = establishDate; }
		public TimeSpan GetActiveTimeSpan() { return DateTime.Now - establishDate; }
		public double GetTotalTransactionsAmount()
		{
			DataBase.OpenConnection();
			var amount = 0.0;
			foreach (Transaction t in DataBase.Transactions().Where(z => z.EnterpriseGuid == guid))
				amount += t.Amount;
			return amount;
		}
	}
}