import pandas as pd

# Read the Excel file in .xls format
excel_file_path = 'cac.xls'
df = pd.read_csv(excel_file_path, header=None, names=['Data'])

# Split the data in column A based on the delimiter "|"
df_split = df['Data'].str.split('|', expand=True)

# Write the new data to a CSV file
csv_file_path = 'csv_file.csv'
df_split.to_csv(csv_file_path, index=False)

print('CSV file created successfully.')
