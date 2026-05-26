# Hybrid AI Helpdesk Ticket Classifier

A C# WinForms desktop application that classifies helpdesk tickets using rule-based logic with an OpenAI API fallback design for low-confidence cases.

## Overview

This project simulates a helpdesk ticket classification workflow. Users enter a support issue, and the application categorizes the ticket by:

- Category
- Priority
- Confidence score
- Suggested support team
- Summary
- Suggested response
- Classification method
- AI status

The project uses a hybrid approach: common support issues are handled by rule-based logic, while low-confidence tickets are designed to trigger an OpenAI fallback layer.

## Key Features

- Rule-based ticket classification for common IT support issues
- OpenAI API fallback design for ambiguous or low-confidence tickets
- Safe fallback handling when the API key is missing, invalid, or unavailable
- AI Status tracking:
  - Not needed
  - Attempted fallback
  - Used
- Classification method tracking:
  - Rule-Based
  - Rule-Based Fallback
  - OpenAI Fallback
- Ticket history table using DataGridView
- CSV save and load functionality
- API key is read from an environment variable instead of being hardcoded

## Supported Ticket Categories

The rule-based classifier can identify common support issues such as:

- Login / Account Access
- Network Issue
- Email / Outlook Issue
- Software Installation Issue
- Hardware Issue
- Payroll / HR System Issue
- Security / Phishing Issue
- General Support

## Hybrid Classification Logic

The application first uses local rule-based logic.

If the ticket matches a known issue pattern, the system returns a rule-based result.

If the rule-based confidence is low, the system attempts to use OpenAI as a fallback.

If the OpenAI API is unavailable or does not return a valid result, the application safely returns the rule-based fallback result instead of crashing.

## Example Workflow

Clear ticket example:

```text
Input: unable to login
Result: Login / Account Access
Classified By: Rule-Based
AI Status: Not needed
