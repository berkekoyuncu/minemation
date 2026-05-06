import PyPDF2
import os

docs_path = r'D:\minemation\docs'
out_path = r'D:\minemation\docs_output_utf8.txt'

with open(out_path, 'w', encoding='utf-8') as f_out:
    for root, _, files in os.walk(docs_path):
        for f in files:
            if f.endswith('.pdf'):
                path = os.path.join(root, f)
                f_out.write(f"=== START OF {f} ===\n")
                try:
                    reader = PyPDF2.PdfReader(path)
                    for page in reader.pages:
                        f_out.write(page.extract_text() + "\n")
                except Exception as e:
                    f_out.write(f"Error reading {f}: {e}\n")
                f_out.write(f"=== END OF {f} ===\n\n")
