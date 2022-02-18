using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankApplication
{

    public delegate CustomerAccountDetails AtmTransactionDelegate(int accountType, CustomerAccountDetails customerAccountDetails);
    public delegate void CustomerBankAccountCreation(CustomerAccountDetails customerAccountDetails);

    #region CustomerAccountDetails class
    public class CustomerAccountDetails
    {
        public int AccountTypeID = 0;
        public string AccountHolderName = string.Empty;
        public string ParentAccountName = string.Empty;
        public string Address = string.Empty;
        public int Age = 0;
        public decimal AccountBalance = 0.0M;
        public int ErrorTypeID = 0;
        public string ErrorMessage = string.Empty;
        public int TransactionPerDay = 0;
        public bool IsATMRequired = false;
        public int CustomerId = 0;
    }
    #endregion

    #region AccountCreation Class
    class AccountCreation
    {
        public void AccountCreationUsingTypeId(CustomerAccountDetails objAD)
        {
            try
            {
                Console.WriteLine();
                switch (objAD.AccountTypeID)
                {
                    case 1:
                        Console.WriteLine("Please Enter Savings Bank Account Details: ");
                        Console.WriteLine("--------------------------------------------");
                        objAD = AcceptCustomerDetails(objAD);
                        break;
                    case 2:
                        Console.WriteLine("Please Enter Current Bank Account Details: ");
                        Console.WriteLine("===========================================");
                        objAD = AcceptCustomerDetails(objAD);
                        break;
                    case 3:
                        Console.WriteLine("Please Enter Child Bank Account Details: ");
                        Console.WriteLine("===========================================");
                        objAD = AcceptCustomerDetails(objAD);
                        break;
                }
            }
            catch (Exception ex)
            {
                objAD.ErrorMessage = "There is an error while creating the account: " + ex.Message;
                objAD.ErrorTypeID = 1;
            }
        }

         public CustomerAccountDetails AcceptCustomerDetails(CustomerAccountDetails accountDetails)
        {
            try
            {
                if (accountDetails.AccountTypeID == 3)
                {
                    Console.Write("Enter Parent Account Holder Name: ");
                    accountDetails.ParentAccountName = Console.ReadLine();
                }
                
                Console.Write("Enter your Name: ");
                accountDetails.AccountHolderName = Console.ReadLine();
                Console.Write("Enter CustomerId: ");
                accountDetails.CustomerId = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter your Age: ");
                accountDetails.Age = Convert.ToInt32(Console.ReadLine());
                Console.Write("Enter your Address: ");
                accountDetails.Address = Console.ReadLine();
                Console.Write("Enter your initial deposit amount: ");
                accountDetails.AccountBalance = Convert.ToDecimal(Console.ReadLine());
                Console.Write("Do you want to continue with ATM Transactions (Enter 1 for Yes. Enter 2 for No): ");
                int ATMId = Convert.ToInt32(Console.ReadLine());
                accountDetails.IsATMRequired = ATMId == 1 ? true : false;
                if (accountDetails.AccountTypeID == 1)
                {
                    if (accountDetails.AccountBalance > 100000)
                    {
                        accountDetails.ErrorTypeID = 2;
                        accountDetails.ErrorMessage = "For Savings Bank Account the limit is 100000, please try again later.\nThank you for Banking with us, have a great day.";

                    }
                }
                else if (accountDetails.AccountTypeID == 2)
                {
                    if (accountDetails.AccountBalance > 200000)
                    {
                        accountDetails.ErrorTypeID = 2;
                        accountDetails.ErrorMessage = "For Current Bank Account the limit is 200000, please try again later.\nThank you for Banking with us, have a great day.";
                    }
                }
                else if (accountDetails.AccountTypeID == 3)
                {
                    if (accountDetails.AccountBalance > 50000)
                    {
                        accountDetails.ErrorTypeID = 2;
                        accountDetails.ErrorMessage = "For Child Bank Account the limit is 50000, please try again later.\nThank you for Banking with us, have a great day.";
                    }
                }
            }
            catch (Exception ex)
            {
                accountDetails.ErrorMessage = "There is an error while creating the account: " + ex.Message;
                accountDetails.ErrorTypeID = 1;
            }
            return accountDetails;
        }
        public void ShowCustomerDetails(CustomerAccountDetails objAcDtls)
        {
            if (objAcDtls.ErrorTypeID > 0)
            {
                Console.WriteLine(objAcDtls.ErrorMessage);
            }
            else
            {
                int id = objAcDtls.AccountTypeID;



                Console.WriteLine("\n-----------------------------------------------------------");
                Console.WriteLine("Congratulations Your Account has been created successfully...");
                Console.WriteLine("-------------------------------------------------------------");
                Console.WriteLine("Account Type\t= {0}", id == 1 ? "Savings" : id == 2 ? "Current" : "Child");
                if (objAcDtls.AccountTypeID == 3)
                {
                    Console.WriteLine("Parent Account Name\t = {0}", objAcDtls.ParentAccountName);
                }
                Console.WriteLine("Account Name\t= {0}", objAcDtls.AccountHolderName);
                Console.WriteLine("Age \t= {0}", objAcDtls.Age);
                Console.WriteLine("Address \t= {0}", objAcDtls.Address);
                Console.WriteLine("A/C Balance\t= {0}", objAcDtls.AccountBalance);
                if (objAcDtls.IsATMRequired)
                {
                    Console.WriteLine("Opted for ATM\t= {0}", objAcDtls.IsATMRequired ? "Yes" : "No");
                }
                Console.WriteLine("-------------------------------------------------------------");
            }
        }

        public CustomerAccountDetails AtmTransactionsCreation(int choice, CustomerAccountDetails objACDtls)
        {
            switch (choice)
            {
                case 1:
                    Console.Write("Please enter amount to be deposited: ");
                    decimal depAmt = Convert.ToDecimal(Console.ReadLine());
                    bool accept = DepositAmount(depAmt, objACDtls);
                    if (accept)
                    {
                        objACDtls.AccountBalance = objACDtls.AccountBalance + depAmt;
                        Console.WriteLine("\nYour Update balance\t= {0}", objACDtls.AccountBalance);
                        objACDtls.TransactionPerDay = objACDtls.TransactionPerDay + 1;
                    }
                    else
                    {
                        Console.WriteLine("Cannot deposit\t" + depAmt + "\t,as It is exceeding your Account Limit");
                        Console.WriteLine("\nYour balance\t= {0}", objACDtls.AccountBalance);
                    }
                    break;
                case 2:
                    Console.Write("Please enter amount to be Withdraw: ");
                    decimal wdAmt = Convert.ToDecimal(Console.ReadLine());
                    bool res = DeductAmount(wdAmt, objACDtls);
                    if (res)
                    {
                        objACDtls.AccountBalance = objACDtls.AccountBalance - wdAmt;
                        Console.WriteLine("\nYour Update balance\t= {0}", objACDtls.AccountBalance);
                        objACDtls.TransactionPerDay = objACDtls.TransactionPerDay + 1;
                    }
                    else
                    {
                        Console.WriteLine("Cannot withdraw\t" +  wdAmt  + "\tAs Your Account Balance is less than entered Withdraw Amount");
                        Console.WriteLine("\nYour balance\t= {0}", objACDtls.AccountBalance);
                    }
                    break;
                case 3:
                    Console.WriteLine("\nYour Available Balance is\t: {0}", objACDtls.AccountBalance);
                    break;
            }



            return objACDtls;
        }

        public bool DepositAmount(decimal dep, CustomerAccountDetails objACDtls)
        {
            bool accept = true;
            switch (objACDtls.AccountTypeID)
            {
                case 1:
                    if (objACDtls.AccountBalance + dep >= 100000)
                    {
                        accept = false;
                    }
                    break;
                case 2:
                    if (objACDtls.AccountBalance + dep >= 200000)
                    {
                        accept = false;
                    }
                    break;
                case 3:
                    if (objACDtls.AccountBalance + dep >= 50000)
                    {
                        accept = false;
                    }
                    break;
            }
            return accept;
        }

        public bool DeductAmount(decimal dep, CustomerAccountDetails objACDtls)
        {
            bool accept = true;
            switch (objACDtls.AccountTypeID)
            {
                case 1:
                    if (objACDtls.AccountBalance - dep <= 0)
                    {
                        accept = false;
                    }
                    break;
                case 2:
                    if (objACDtls.AccountBalance - dep <= 0)
                    {
                        accept = false;
                    }
                    break;
                case 3:
                    if (objACDtls.AccountBalance - dep <= 0)
                    {
                        accept = false;
                    }
                    break;
            }
            return accept;
        }

        public void ShowAllCustomerDetails(List<CustomerAccountDetails> accountsList)
        {
            string filepath = @"C:\Users\adminvm.adminvm\source\repos\Files\CustomerDetails.txt";
            using (StreamWriter sw = File.AppendText(filepath))
            {
                int SNO = 0;
                Console.WriteLine("*---------------------------------------------------------------------------------------*");
                Console.WriteLine("||CustomerID | A/C Type | A/C Name | Age | Address | Balance | ATM ||");
                Console.WriteLine("*---------------------------------------------------------------------------------------*");
                sw.WriteLine("*--------------------------------------------------------------------------------------------*");
                sw.WriteLine("||CustomerID | A/C Type | A/C Name | Age | Address | Balance | ATM ||");
                sw.WriteLine("*--------------------------------------------------------------------------------------------*");
                foreach (CustomerAccountDetails objAC in accountsList)
                {
                    SNO = SNO + 1;
                    int id = objAC.AccountTypeID;
                    Console.Write(" || " +
                    objAC.CustomerId + " | " +  
                    (id == 1 ? "Savings" : (id == 2 ? "Current" : "Child ")) + " | " +
                    objAC.AccountHolderName + " | " + objAC.Age + " | " + objAC.Address + " | " + objAC.AccountBalance + " | " +
                    (objAC.IsATMRequired ? "Yes" : "NO ") +  " || "
                    );
                    Console.WriteLine("\n*---------------------------------------------------------------------------------------*");



                    sw.WriteLine(" || " +
                    objAC.CustomerId + " | " +
                    (id == 1 ? "Savings" : (id == 2 ? "Current" : "Child ")) + " |" +
                    objAC.AccountHolderName + " \t "  + " | " + objAC.Age + " \t " + " | " + objAC.Address + " \t " + " | " + objAC.AccountBalance + " \t" + " | " +
                    (objAC.IsATMRequired ? "Yes" : "NO ")  + " || "
                    );
                    sw.WriteLine(Environment.NewLine + "*-----------------------------------------------------------------------*");

                }
                sw.Close();
            }
        }

        public void TransactionLimitExceeding(CustomerAccountDetails objACDtls)
        {
            Console.WriteLine("\nYou have reached daily transaction limit (i.e: 3), so Amount of 500 is being deducted from your account.");
            Console.WriteLine("Balance before deductions\t = {0}", objACDtls.AccountBalance);
            objACDtls.AccountBalance = objACDtls.AccountBalance - 500;
            Console.WriteLine("Available balance\t = {0}", objACDtls.AccountBalance);
        }
    }
    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            AccountCreation accountCreation = new AccountCreation();
            int accountType = 0;
            List<CustomerAccountDetails> customerAccountDetailsList = new List<CustomerAccountDetails>();
            CustomerAccountDetails customerAccountDetails = null;
            try
            {
                while (accountType >= 0)
                {
                    if (accountType == 0)
                    {
                        Console.Write("1. Enter 1 to open Savings Bank A/C." +
                            "\n2. Enter 2 to open Current Bank A/C." +
                            "\n3. Enter 3 to open Child Bank A/C." +
                            "\nPlease choose your account type: ");
                        accountType = Convert.ToInt32(Console.ReadLine());
                        customerAccountDetails = new CustomerAccountDetails();
                        customerAccountDetails.AccountTypeID = accountType;
                    }
                    if (accountType > 0 && accountType <= 3)
                    {
                        CustomerBankAccountCreation objCrtCstData = new CustomerBankAccountCreation(accountCreation.AccountCreationUsingTypeId);
                        objCrtCstData += accountCreation.ShowCustomerDetails;
                        objCrtCstData(customerAccountDetails);
                        if ((accountType == 1 && customerAccountDetails.AccountBalance <= 100000) || (accountType == 2 && customerAccountDetails.AccountBalance <= 200000) || (accountType == 3 && customerAccountDetails.AccountBalance <= 50000))
                        {
                            if (customerAccountDetails.IsATMRequired)
                            {
                                int intoperation = 0;
                                while (intoperation >= 0 && intoperation <= 3)
                                {
                                    Console.WriteLine("\nAs you have opted for ATM Transactions, please select below options to perform:" +
                                        "\n1. Deposit." +
                                        "\n2. Withdraw." +
                                        "\n3. Check Available Balance." +
                                        "\n4. Exit ATM.");
                                    Console.Write("Enter your choice: ");
                                    intoperation = Convert.ToInt32(Console.ReadLine());
                                    if (intoperation >= 1 && intoperation <= 3)
                                    {
                                        AtmTransactionDelegate objDel = new AtmTransactionDelegate(accountCreation.AtmTransactionsCreation);
                                        customerAccountDetails = objDel(intoperation, customerAccountDetails);
                                        if (customerAccountDetails.TransactionPerDay > 3)
                                        {
                                            accountCreation.TransactionLimitExceeding(customerAccountDetails);
                                        }
                                    }
                                    if (intoperation < 4)
                                    {
                                        Console.WriteLine("\n\n-------------------------------------------------------------");
                                        Console.WriteLine("Enter 0 to do another transaction or enter 4 to exit ATM.");
                                        intoperation = Convert.ToInt32(Console.ReadLine());
                                    }
                                };
                            }
                            customerAccountDetailsList.Add(customerAccountDetails);
                        }
                    }
                    else if (accountType > 3)
                    {
                        accountCreation.ShowAllCustomerDetails(customerAccountDetailsList);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Choice...");
                    }
                    Console.WriteLine("\n\n-----------------------------------------------------------------");
                    Console.WriteLine("Enter 0 to Create account.\nEnter 4 to display all customer details.\nEnter -1 to exit.");
                    Console.Write("Enter Your choice: ");
                    accountType = Convert.ToInt32(Console.ReadLine());
                    Console.WriteLine();
                    Console.WriteLine("-----------------------------------------------------------------------\n");


                };
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nInvalid input, please try again later...");
                Console.ReadKey();
            }
        }
    }
}


