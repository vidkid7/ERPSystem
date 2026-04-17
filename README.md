# Dynamic ERP System Scraper

Automation scripts to login and extract information from Dynamic ERP demo system.

## Two Versions Available

### 1. Simple Version (requests + BeautifulSoup)
- File: `dynamic_erp_scraper.py`
- Lightweight, no browser required
- May have limitations with JavaScript-heavy sites

### 2. Browser Automation (Selenium)
- File: `dynamic_erp_selenium.py`
- Full browser automation
- Handles JavaScript and dynamic content
- **Recommended for best results**

## Setup

### Install Dependencies

```bash
pip install -r requirements.txt
```

### For Selenium Version (Recommended)

You'll also need Chrome browser installed. The script will automatically download ChromeDriver.

## Usage

### Option 1: Run Simple Scraper
```bash
python dynamic_erp_scraper.py
```

### Option 2: Run Selenium Scraper (Recommended)
```bash
python dynamic_erp_selenium.py
```

### Option 3: Windows Batch File
```bash
run_scraper.bat
```

## What It Does

The scraper will:
1. Login to Dynamic ERP demo system
2. Navigate to the dashboard
3. Extract all menu items and navigation
4. Identify available modules and features
5. Explore key pages automatically
6. Save all information to JSON file

## Output Files

- `dynamic_erp_info.json` - Simple scraper output
- `dynamic_erp_selenium_info.json` - Selenium scraper output

Contains:
- System information
- Menu structure
- Available modules
- Page titles and headings
- Buttons and forms
- Navigation links

## Configuration

Edit the script to change credentials:
```python
BASE_URL = 'https://mktdemo.dynamicerp.online'
USERNAME = 'Admin'
PASSWORD = 'DynamicDemo@9909#'
```

## Troubleshooting

If Selenium fails:
1. Make sure Chrome browser is installed
2. Install webdriver-manager: `pip install webdriver-manager`
3. Or manually download ChromeDriver from https://chromedriver.chromium.org/

## Security Note

This script is designed for demo/testing environments only. Never use it on production systems without proper authorization.

