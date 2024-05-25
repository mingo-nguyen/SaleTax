import argparse
import pandas as pd

def convert_excel_to_csv(excel_file_path, csv_file_path):
    # Read the Excel file in .xls format
    with open(excel_file_path, 'r') as file:
        # Read lines and split the data based on the delimiter "|"
        data = [line.strip().split('|') for line in file.readlines()]

    # Remove empty rows
    data = [row for row in data if any(cell.strip() for cell in row)]

    # Convert to a DataFrame
    df = pd.DataFrame(data)

    # Write the new data to a CSV file
    df.to_csv(csv_file_path, index=False)
    print('CSV file created successfully.')

if __name__ == "__main__":
    # Parse command-line arguments
    parser = argparse.ArgumentParser(description='Convert Excel (.xls) to CSV')
    parser.add_argument('excel_file_path', help='Path to the Excel file (.xls)')
    parser.add_argument('csv_file_path', help='Desired path for the output CSV file')

    args = parser.parse_args()

    # Convert the Excel file to CSV
    convert_excel_to_csv(args.excel_file_path, args.csv_file_path)
