Design:
The ReceiptGenerator acts as the main driver of this application. It consumes a string created by user using CLI.
Upon receiving an item string, it parses pertinent data and creates a new Receipt object which is stored as an internal List.
When a receipt object is created, it calculates and formats applicable taxes.

The ReceiptGenerator uses a list of strings to determine if an item is sales tax exempt or not. In the real world, I imagine this would be pulled from a database. For this purpose, it is just a List of strings.

How to use:

To use, start the application and enter each item and then enter. The item must be entered in the same format as defined in the instructions.
Once all items are entered, type "done" and the receipt will then be generated.