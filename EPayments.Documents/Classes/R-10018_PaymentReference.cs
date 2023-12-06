//--------------------------------------------------------------
// Autogenerated by XSDObjectGen version 1.0.0.0
//--------------------------------------------------------------

using System;
using System.Xml.Serialization;
using System.Collections;
using System.Xml.Schema;
using System.ComponentModel;

namespace R_10018
{

	public struct Declarations
	{
		public const string SchemaVersion = "http://ereg.egov.bg/segment/R-10018";
	}




	[XmlType(TypeName="PaymentReference",Namespace=Declarations.SchemaVersion),Serializable]
	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public partial class PaymentReference
	{

		[System.Web.Script.Serialization.ScriptIgnore]
        [Newtonsoft.Json.JsonIgnore]
		[XmlElement(ElementName="PaymentReferenceType",IsNullable=false,Form=XmlSchemaForm.Qualified,DataType="string",Namespace=Declarations.SchemaVersion)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string __PaymentReferenceType;
		
		[XmlIgnore]
		public string PaymentReferenceType
		{ 
			get { return __PaymentReferenceType; }
			set { __PaymentReferenceType = value; }
		}

		[System.Web.Script.Serialization.ScriptIgnore]
        [Newtonsoft.Json.JsonIgnore]
		[XmlElement(ElementName="PaymentReferenceNumber",IsNullable=false,Form=XmlSchemaForm.Qualified,DataType="string",Namespace=Declarations.SchemaVersion)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public string __PaymentReferenceNumber;
		
		[XmlIgnore]
		public string PaymentReferenceNumber
		{ 
			get { return __PaymentReferenceNumber; }
			set { __PaymentReferenceNumber = value; }
		}

		[System.Web.Script.Serialization.ScriptIgnore]
        [Newtonsoft.Json.JsonIgnore]
		[XmlElement(ElementName="PaymentReferenceDate",Form=XmlSchemaForm.Qualified,DataType="date",Namespace=Declarations.SchemaVersion)]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public DateTime? __PaymentReferenceDate;
		
		[System.Web.Script.Serialization.ScriptIgnore]
        [Newtonsoft.Json.JsonIgnore]
		[XmlIgnore]
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public bool __PaymentReferenceDateSpecified { get { return __PaymentReferenceDate.HasValue; } }
		
		[XmlIgnore]
		public DateTime? PaymentReferenceDate
		{ 
			get { return __PaymentReferenceDate; }
			set { __PaymentReferenceDate = value; }
		}
		


		public PaymentReference()
		{
		}
	}
}