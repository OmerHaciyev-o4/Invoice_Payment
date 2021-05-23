using System;
using System.IO;
using Newtonsoft.Json;

namespace InvoicePayment
{
    public class InvoicePayment
    {
        public static bool SeriAndDesri;
        public double PaymentDay { get; init; }                         // gunluk odenilecek mebleg
        public int NumberDays { get; private set; } = 180;              // nece gunu var
        public double PenaltyForOneDayPaymentDelay { get; init; }       // bir gun odenilmeyende cerimesi
        public int AmountToPaidWithoutPenalty { get; private set; }     // nece gun odenilmeyi onun sayi
        public int Penalty { get; private set; }                        //AmountToPaidWithoutPenalty --> qiymet neticesi   
        public DateTime MoneyTakenTime { get; private set; }            // pulu goturduyu zaman
        public DateTime PaymentDateTime { get; private set; }           // pulu goturduyu zaman bir gun sonrani gosterir
        public int TotalPaymentAmount { get; private set; }             // goturduyu pul
        public double RemainingAmount { get; private set; }             // goturduyu meblegin ne qeder qaldigini gosterir.


        public InvoicePayment(int amount)
        {
            TotalPaymentAmount = amount;
            RemainingAmount = amount;
            PaymentDay = amount / NumberDays;
            PenaltyForOneDayPaymentDelay = 4.5;
            MoneyTakenTime = DateTime.Now;
            PaymentDateTime = DateTime.Now.AddDays(1);
            this.print();
        }

        private void print() { Console.WriteLine($"Every day you will have to pay {PaymentDay}"); }

        private int dateTimeCalculator()
        {
            if (DateTime.Now.Year == PaymentDateTime.Year)
            {
                if (DateTime.Now.Month == PaymentDateTime.Month)
                {
                    if (DateTime.Now.Day > PaymentDateTime.Day) { return -1; }
                    else if (DateTime.Now.Day == PaymentDateTime.Day) { return -1; }
                    else { return -2; }
                }
                else if (DateTime.Now.Month > PaymentDateTime.Month)
                {
                    if (DateTime.Now.Day < PaymentDateTime.Day) { return -1; }
                    else if (DateTime.Now.Day == PaymentDateTime.Day) { return 0; }
                    else if (DateTime.Now.Day >= PaymentDateTime.Day) { return 0; }
                    else { return -2; }
                }
                else { return -2; }
            }
            else if (DateTime.Now.Year >= PaymentDateTime.Year)
            {
                if (DateTime.Now.Month == PaymentDateTime.Month)
                {
                    if (DateTime.Now.Day < PaymentDateTime.Day) { return -1; }
                    else if (DateTime.Now.Day == PaymentDateTime.Day) { return 0; }
                    else if (DateTime.Now.Day > PaymentDateTime.Day) { return 1; }
                    else { return -2; }
                }
                else if (DateTime.Now.Month > PaymentDateTime.Month) { return 1; }
                else { return -2; }
            }
            else { return -2; }
        }

        private int penaltyCalculator()
        {
            int temp, temp1;
            if (DateTime.Now.Year == PaymentDateTime.Year)
            {
                if (DateTime.Now.Month > PaymentDateTime.Month)
                {
                    if (DateTime.Now.Day == PaymentDateTime.Day) { return 0; }
                    else if (DateTime.Now.Day > PaymentDateTime.Day) { return PaymentDateTime.Day - DateTime.Now.Day; }
                    else { return 0; }
                }
                else { return -2; }
            }
            else if (DateTime.Now.Year > PaymentDateTime.Year)
            {
                if (DateTime.Now.Month < PaymentDateTime.Month)
                {
                    temp1 = (12 - PaymentDateTime.Month) + DateTime.Now.Month;
                    temp = temp1 * 30;
                    if (DateTime.Now.Day < PaymentDateTime.Day)
                    {
                        temp1 = 30 - DateTime.Now.Day;
                        temp -= temp1;
                        return temp;
                    }
                    else if (DateTime.Now.Day == PaymentDateTime.Day)
                    {
                        temp1 = 30 - DateTime.Now.Day;
                        temp -= temp1;
                        return temp;
                    }
                    else
                    {
                        temp1 = DateTime.Now.Day - PaymentDateTime.Day;
                        temp += temp1;
                        return temp;
                    }
                }
                else if (DateTime.Now.Month == PaymentDateTime.Month)
                {
                    temp1 = 12 * 30;
                    temp = temp1 * 30;
                    if (DateTime.Now.Day < PaymentDateTime.Day)
                    {
                        temp1 = 30 - DateTime.Now.Day;
                        temp -= temp1;
                        return temp;
                    }
                    else if (DateTime.Now.Day == PaymentDateTime.Day)
                    {
                        temp1 = 30 - DateTime.Now.Day;
                        temp -= temp1;
                        return temp;
                    }
                    else
                    {
                        temp1 = DateTime.Now.Day - PaymentDateTime.Day;
                        temp += temp1;
                        return temp;
                    }
                }
                else
                {
                    temp1 = (12 - PaymentDateTime.Month) + DateTime.Now.Month; ;
                    temp = temp1 * 30;
                    if (DateTime.Now.Day < PaymentDateTime.Day)
                    {
                        temp1 = 30 - DateTime.Now.Day;
                        temp -= temp1;
                        return temp;
                    }
                    else if (DateTime.Now.Day == PaymentDateTime.Day)
                    {
                        temp1 = 30 - DateTime.Now.Day;
                        temp -= temp1;
                        return temp;
                    }
                    else
                    {
                        temp1 = DateTime.Now.Day - PaymentDateTime.Day;
                        temp += temp1;
                        return temp;
                    }
                }
            }
            else { return -2; }
        }

        public double MakePayment(double amount)
        {
            if (NumberDays <= 180)
            {
                if (this.dateTimeCalculator() == 0)
                {
                    PaymentDateTime.AddDays(1);
                    if (amount == PaymentDay) { RemainingAmount -= amount; Console.WriteLine("Debt paid"); return 0; }
                    else if (amount < PaymentDay) { Console.WriteLine($"You need up to {PaymentDay - amount} to make the payment."); return 0; }
                    else if (amount >= PaymentDay) { RemainingAmount -= PaymentDay; return amount - PaymentDay; }
                    else { return 0; }
                }
                else if (this.dateTimeCalculator() == -1) { RemainingAmount -= PaymentDay; return amount - PaymentDay; }
                else if (this.dateTimeCalculator() == 1)
                {
                    int temp = this.penaltyCalculator();
                    if (temp != -2 && temp > 0)
                    {
                        PaymentDateTime.AddMonths(1);
                        if (amount == PaymentDay)
                        {
                            Console.WriteLine($"Please send more money as far as {temp * PenaltyForOneDayPaymentDelay}. Reason: Did you delay payment {temp} days.");
                            NumberDays -= 1; RemainingAmount -= amount; return 0;
                        }
                        else if (amount >= PaymentDay)
                        {
                            if (amount == temp * PenaltyForOneDayPaymentDelay)
                            {
                                NumberDays -= 1;
                                RemainingAmount -= amount;
                                return 0;
                            }
                            else if (amount >= temp * PenaltyForOneDayPaymentDelay) { RemainingAmount -= PaymentDay; return amount - PaymentDay; }
                        }
                    }
                }
            }
            else { Console.WriteLine("Payment finish"); }
            return 0;
        }

        /// <summary>
        /// first evaluate the variable  
        /// <param>SeriAndDesri</param>
        /// </summary>
        public static void SAD(InvoicePayment obj)
        {
            if (SeriAndDesri == true)
            {
                string returning = JsonConvert.SerializeObject(obj);
                File.WriteAllLines("InvoicePayment.json", new string[] { returning });

                var output = File.ReadAllLines("InvoicePayment.json");

                var line = output[0];

                var result = JsonConvert.DeserializeObject(line);

                Console.WriteLine(result);
            }
            else
            {
                dynamic obj1 = new { PaymentDay = obj.PaymentDay, NumberDays = obj.NumberDays, PenaltyForOneDayPaymentDelay = obj.PenaltyForOneDayPaymentDelay };

                string returning = JsonConvert.SerializeObject(obj1);

                File.WriteAllLines("result.json", new string[] { returning });

                var output = File.ReadAllLines("result.json");

                var line = output[0];

                var result = JsonConvert.DeserializeObject(line);

                Console.WriteLine(result);
            }
        }
    }
}