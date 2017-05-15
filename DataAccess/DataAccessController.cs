using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;

namespace DataAccess

{
  public  class DataAccessController
    {

        //get Category
        public static List<Card> getCard()
        {
            List<Card> categoryList = null;
            try
            {
                GridEntryEntities1 db = new GridEntryEntities1();
                categoryList = db.Cards.ToList();
                db.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return categoryList;
        }

      public static int getInvCount()
      {
          GridEntryEntities1 db = new GridEntryEntities1();
          int l_count = db.Pur_Inv_His.Count();
          db.Dispose();
          return l_count;
      }

      public static double getPrice(int cardId)
      {
          GridEntryEntities1 db = new GridEntryEntities1();

          double l_price = Convert.ToDouble((from card in db.Cards
                                 where card.CardID.Equals(cardId)
                                 select card.CardAmount).SingleOrDefault());
          db.Dispose();
          return l_price;
      }


      //Master and Detail INSERT
      public static int SaveInvDAO(Pur_Inv_His InvObj, DataTable dt)
      {
          try
          {

              GridEntryEntities1 db = new GridEntryEntities1();

              var entity = db.Set<Pur_Inv_His>().Create();

              //mapping the values of your view models to data models
              entity.VouNo = InvObj.VouNo;
              entity.SupplierName = InvObj.SupplierName;
              entity.PurDate = InvObj.PurDate;
              entity.Remark = InvObj.Remark;

              //INSERT MASTER TABLE
              db.Pur_Inv_His.Add(entity);
              int l_return =  db.SaveChanges();
              int purID = entity.PurID; //GET MASTER ID

              if(l_return  > 0)
              {
                  //INSERT DETAIL TABLE
                  for(int i=0; i <dt.Rows.Count; i++)
                  {
                      var PurDetail = db.Set<Pur_Detail_His>().Create();
                      PurDetail.SrNo = Convert.ToInt32(dt.Rows[i]["No"]);
                      PurDetail.PurID = purID; //SET MASTER ID FROM RETURN ID AFTER INSERT MASTER
                      PurDetail.CardID = Convert.ToInt32( dt.Rows[i]["CardTypeId"]);
                      PurDetail.Amount = Convert.ToDecimal( dt.Rows[i]["Amount"]);
                      PurDetail.Qty = dt.Rows[i]["QTY"].ToString();
                      PurDetail.TotalAmount = Convert.ToDecimal(dt.Rows[i]["TotalAmount"]);
                      db.Pur_Detail_His.Add(PurDetail);
                     l_return = db.SaveChanges();
                      PurDetail = null;

                  }
              }

              db.Dispose();

              return l_return;
          }
          catch (Exception ex)
          {
              throw ex;
          }
      }


      //Search Purchse Item List
      public static DataTable getPurchaseItemList(String FromDate, String ToDate, int categroy_id)
      {
          List<Pur_Detail_His> bookList = null;
          DataTable dt = new DataTable();
          try
          {
              GridEntryEntities1 db = new GridEntryEntities1();
          
              var fromDate = new SqlParameter("@getFDate", FromDate.Trim());
              var toDate = new SqlParameter("@getTDate", ToDate.Trim());
              var category = new SqlParameter("@getCardID", categroy_id);
       

              bookList = db.Database
                   .SqlQuery<Pur_Detail_His>("SelectPur_List @getFDate , @getTDate, @getCardID "
                   , fromDate, toDate, category)
                   .ToList();
              dt.Columns.Add("PurID");
              dt.Columns.Add("SrNo");
              dt.Columns.Add("CardType");
              dt.Columns.Add("CardAmount");
              dt.Columns.Add("QTY");
              dt.Columns.Add("TotalAmount");


              for (int i = 0; i < bookList.Count; i++)
              {

                  int cardId = bookList[i].CardID;
                  var Obj = db.Cards.First(a => a.CardID == cardId);

                  DataRow dr = dt.NewRow();
                  dr["PurID"] = bookList[i].PurID;
                  dr["SrNo"] = bookList[i].SrNo;
                  dr["CardType"] = Obj.CardType;
                  dr["CardAmount"] = Obj.CardAmount;
                  dr["QTY"] = bookList[i].Qty;
                  dr["TotalAmount"] = bookList[i].TotalAmount;
                  dt.Rows.Add(dr);
                  dr = null;
              }

                  db.Dispose();
          }
          catch (Exception ex)
          {
              throw ex;
          }
          return dt;
      }

    }
}
