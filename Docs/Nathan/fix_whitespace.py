"""
Python script for replacing tabs with spaces in src files.
Run with `python fix_whitespace.py`.

"""
import pathlib
import os

PATH = pathlib.Path("./src/")

if __name__ == "__main__":
    for root, dirs, files in os.walk(str(PATH)):
        for file in files:
            file_path = pathlib.Path(root) / pathlib.Path(file)
            if str(file_path).endswith(".cs"):
                data = ""
                with open(file_path, "r", encoding="utf-8") as fd:
                    data = fd.read()
                if "\t" in data:
                    number = [x for x in data if x == "	"]
                    print(f"Replacing {len(number)} in file {file_path}")
                    data = data.replace("\t", "    ")
                    with open(file_path, "w", encoding="utf-8") as fd:
                        fd.write(data)
