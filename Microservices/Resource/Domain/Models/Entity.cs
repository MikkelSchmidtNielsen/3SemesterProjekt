using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
	/// <summary>
	/// Should be implemented for all Entities which want concurrency
	/// </summary>
	public abstract class Entity
	{
		// Property for concurrency
		[Timestamp]
		public byte[] RowVersion { get; protected set; } = BitConverter.GetBytes(1UL);

		/// <summary>
		/// Method which automaticly sets the new rowversion as a ulong and then converts it back to byte[]
		/// </summary>
		public void IncrementRowVersion()
		{
			ulong current = BitConverter.ToUInt64(RowVersion, 0);
			RowVersion = BitConverter.GetBytes(current + 1);
		}
	}
}
