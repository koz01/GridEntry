using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DataAccess;

namespace BussinessLogicLayer
{
   public class DataLogicController
   {
       DataTable dt;
       DataAccessController getDataAccess = new DataAccessController();

       public DataTable getCardRef()
       {
           dt = new DataTable();
           return dt = getDataAccess.getCardType();
       }

       public string getAmout(string CatchID)
       {
           string strAmt = getDataAccess.getAmt(CatchID);
           return strAmt;
       }
    
   }    

}
