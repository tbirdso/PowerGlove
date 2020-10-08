# Filename: combine_leap_data.py
# Written by:   Tom Birdsong
# Course:       ECE 4960
# Purpose:      Combine leap motion data into a single dataset for NN training


import csv
import pandas as pd

DATAPATH = '..\Power Glove Project\Data\Leap'
FILENAME = 'compiled_leap_noext.csv'
LOW_LABEL = 1
HIGH_LABEL = 10

# Flag to use boolean values for finger extended or not
useExtended = False

# Structure: folders 1 - 10, file for each finger
FOLDERS = [str(i) for i in range(LOW_LABEL, HIGH_LABEL + 1)]
CSV_NAME = '10_6_{0}.csv'
FINGERS = ['thumb','index', 'middle', 'ring', 'pinky']

# Compiled dataframe
compiled_df = pd.DataFrame()

# Retrieve and append data
for dir in FOLDERS:
    label_df = pd.DataFrame()
    for finger in FINGERS:

        right_df = pd.read_csv(DATAPATH + '\\' + dir + '\\' + CSV_NAME.format(finger))
        print('Imported ' + str(len(right_df)) + ' records from ' + dir + finger)
        
        # Drop unused columns
        right_df = right_df.drop(labels=['bones', 'Type', 'Id', 'HandId', 'TimeVisible'],axis=1)

        # Convert columns to numeric
        if(useExtended):
            right_df['IsExtended'] = right_df['IsExtended'].apply(lambda val : 1 if val else 0)
        else:
            right_df = right_df.drop(labels=['IsExtended'], axis=1)

        # Rename columns with finger
        cols = list(right_df)
        new_cols = list()
        for col in cols:
            new_cols.append(finger + '_' + col)

        right_df.columns = new_cols

        # Append to compiled data
        label_df = pd.concat([label_df, right_df], axis=1)

    label_df['Label'] = dir
    compiled_df = pd.concat([compiled_df, label_df], axis=0, ignore_index=True)

# print('Headers: ' + str(list(compiled_df)))
print('Num features: ' + str(len(list(compiled_df)) - 1) + '; Num records: ' + str(len(compiled_df)))

compiled_df.to_csv(path_or_buf=DATAPATH + '\\' + FILENAME, index=False)
