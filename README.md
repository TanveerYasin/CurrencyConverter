# CurrencyConverterApi
Project Details:
First endpoint -> Retrieve the latest exchange rates for a specific base currency (e.g., EUR). 
Second endpoint -> Allow users to convert amounts between different currencies. In case of TRY, PLN, THB, and MXN currency conversions, the endpoint should return a bad response and these currencies should be excluded from response. (These currencies should be excluded only for this endpoint! ) 
Third endpoint -> Return a set of historical rates for a given period using pagination based on a specific base currency. (e.g., 2020-01-01..2020-01-31, base EUR)

How to Run project:
1.Intsall Visual Studio 2022. 
2.Clone project in any local folder in your computer.
3.Open solution in visual studio 2022 or more as project required .netframework 7.0 not less than that. 
4.Run project through IIS express on local browser.
5.Navigate to swagger index page by entering this in end of your localhost URL : https://localhost:(PortNumber)/swagger/index.html
6.Enter required data through swagger inputs and you will get desired results. 

Execute Test Case:
You can also run test cases in Test project there are Unit test written for some of possibile outcomes and we can make more test cases as we want
