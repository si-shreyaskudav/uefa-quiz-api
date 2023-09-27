using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;

namespace Gaming.Quiz.DataInitializer.Common
{
    public class Utility
    {

        public static Int64[] GetPagePoints(Int64 pageOneChunk, Int64 pageChunk, Int64 pageNo)
        {
            Int64[] address = new Int64[2];
            Int64 mPageOneSize = pageOneChunk;
            Int64 mCurrPageSize = pageChunk;
            Int64 mPageNo = pageNo;
            Int64 mFrom = 0;
            Int64 mTo = 0;
            mTo = mPageOneSize + ((mPageNo - 1) * mCurrPageSize);
            if (mPageNo == 1)
                mFrom = mTo - mPageOneSize;
            else
                mFrom = mTo - mCurrPageSize;
            mFrom = mFrom + 1;
            address[0] = mFrom;
            address[1] = mTo;
            return address;
        }

        public static DataSet GetDataSet(NpgsqlCommand mNpgsqlCmd, List<String> cursors)
        {
            DataSet ds = new DataSet();

            using (NpgsqlDataAdapter da = new NpgsqlDataAdapter())
            {
                foreach (String cursor in cursors)
                {
                    mNpgsqlCmd.CommandText = "fetch all in \"" + cursor + "\"";
                    mNpgsqlCmd.CommandType = CommandType.Text;
                    mNpgsqlCmd.CommandTimeout = 0;

                    da.SelectCommand = mNpgsqlCmd;

                    DataTable dt = new DataTable(cursor);
                    da.Fill(dt);
                    ds.Tables.Add(dt);
                }
            }

            return ds;
        }

        public static String MemberNotation(Int64 count)
        {
            String notation = count.ToString();

            if (count > 0 && count < 1000)
                notation = count.ToString();
            if (count > 999 && count < 5000)
                notation = "1k+";
            if (count > 4999 && count < 10000)
                notation = "5k+";
            if (count > 9999 && count < 20000)
                notation = "10k+";
            if (count > 19999 && count < 30000)
                notation = "20k+";
            if (count > 29999 && count < 40000)
                notation = "30k+";
            if (count > 39999 && count < 50000)
                notation = "40k+";
            if (count > 49999 && count < 60000)
                notation = "50k+";
            if (count > 59999 && count < 70000)
                notation = "60k+";
            if (count > 69999 && count < 80000)
                notation = "70k+";
            if (count > 79999 && count < 90000)
                notation = "80k+";
            if (count > 89999 && count < 100000)
                notation = "90k+";
            if (count > 99999 && count < 200000)
                notation = "100k+";
            if (count > 199999 && count < 300000)
                notation = "200k+";
            if (count > 299999 && count < 400000)
                notation = "300k+";
            if (count > 399999 && count < 500000)
                notation = "400k+";
            if (count > 499999 && count < 600000)
                notation = "500k+";
            if (count > 599999 && count < 700000)
                notation = "600k+";
            if (count > 699999 && count < 800000)
                notation = "700k+";
            if (count > 799999 && count < 900000)
                notation = "800k+";
            if (count > 899999 && count < 100000)
                notation = "900k+";
            if (count > 999999)
                notation = "1m+";

            return notation;
        }

        public static String MemberNotation2(Int64 count)
        {
            String notation = count.ToString();

            if (count > 0 && count < 1000)
                notation = count.ToString();
            if (count > 999 && count < 2000)
                notation = "1k+";
            if (count > 1999 && count < 3000)
                notation = "2k+";
            if (count > 2999 && count < 4000)
                notation = "3k+";
            if (count > 3999 && count < 5000)
                notation = "4k+";
            if (count > 4999 && count < 6000)
                notation = "5k+";
            if (count > 5999 && count < 7000)
                notation = "6k+";
            if (count > 6999 && count < 8000)
                notation = "7k+";
            if (count > 7999 && count < 9000)
                notation = "8k+";
            if (count > 8999 && count < 10000)
                notation = "9k+";
            if (count > 9999 && count < 11000)
                notation = "10k+";
            if (count > 10999 && count < 12000)
                notation = "11k+";
            if (count > 11999 && count < 13000)
                notation = "12k+";
            if (count > 12999 && count < 14000)
                notation = "13k+";
            if (count > 13999 && count < 15000)
                notation = "14k+";
            if (count > 14999 && count < 16000)
                notation = "15k+";
            if (count > 15999 && count < 17000)
                notation = "16k+";
            if (count > 16999 && count < 18000)
                notation = "17k+";
            if (count > 17999 && count < 19000)
                notation = "18k+";
            if (count > 18999 && count < 20000)
                notation = "19k+";
            if (count > 19999 && count < 21000)
                notation = "20k+";
            if (count > 20999 && count < 22000)
                notation = "21k+";
            if (count > 21999 && count < 23000)
                notation = "22k+";
            if (count > 22999 && count < 24000)
                notation = "23k+";
            if (count > 23999 && count < 25000)
                notation = "24k+";
            if (count > 24999 && count < 26000)
                notation = "25k+";
            if (count > 25999 && count < 27000)
                notation = "26k+";
            if (count > 26999 && count < 28000)
                notation = "27k+";
            if (count > 27999 && count < 29000)
                notation = "28k+";
            if (count > 28999 && count < 30000)
                notation = "29k+";
            if (count > 29999 && count < 31000)
                notation = "30k+";

            //if (count > 9999 && count < 20000)
            //    notation = "10k+";
            //if (count > 19999 && count < 30000)
            //    notation = "20k+";
            if (count > 30999 && count < 40000)
                notation = "31k+";
            if (count > 39999 && count < 50000)
                notation = "40k+";
            if (count > 49999 && count < 60000)
                notation = "50k+";
            if (count > 59999 && count < 70000)
                notation = "60k+";
            if (count > 69999 && count < 80000)
                notation = "70k+";
            if (count > 79999 && count < 90000)
                notation = "80k+";
            if (count > 89999 && count < 100000)
                notation = "90k+";
            if (count > 99999 && count < 200000)
                notation = "100k+";
            if (count > 199999 && count < 300000)
                notation = "200k+";
            if (count > 299999 && count < 400000)
                notation = "300k+";
            if (count > 399999 && count < 500000)
                notation = "400k+";
            if (count > 499999 && count < 600000)
                notation = "500k+";
            if (count > 599999 && count < 700000)
                notation = "600k+";
            if (count > 699999 && count < 800000)
                notation = "700k+";
            if (count > 799999 && count < 900000)
                notation = "800k+";
            if (count > 899999 && count < 100000)
                notation = "900k+";
            if (count > 999999)
                notation = "1m+";

            return notation;
        }

        public static String StatNotation(Int64 count)
        {
            String notation = count.ToString();

            if (count > 999)
                notation = ((float)count / 1000).ToString("#.#") + "k";

            return notation;
        }
    }
}
