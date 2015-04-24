define([], function() {
    // queue column definitions
    return {
        "QueueColumns" : {
            "Default": [
                { "columnName": "ShortName", "displayName": "Name" },
                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ],
            "QAQueues": [
                { "columnName": "ShortName", "displayName": "Name" },
                { "columnName": "LoadId", "displayName": "Load Id" },

                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ],
            "NMECQueues": [
                { "columnName": "ShortName", "displayName": "Name" },
                { "columnName": "BatchName", "displayName": "Batch Name" },
                { "columnName": "Priority", "displayName": "Priority" },
                { "columnName": "MediaLength", "displayName": "Media Length" },
                { "columnName": "TotalPages", "displayName": "Page Count" },
                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ],
            "SectionErrorQueues": [
                { "columnName": "ShortName", "displayName": "Name" },
                { "columnName": "ParentHarmonyNumber", "displayName": "Parent Harmony Number" },
                { "columnName": "LoginName", "displayName": "Assigned User" },
                { "columnName": "StatusChanged", "displayName": "Assign Date" },
                { "columnName": "DateSubmitted", "displayName": "Date Submitted" }
            ]
        },
        "QueueColumnMapping": {
            "Text Extraction Error": "SectionErrorQueues",
            "NEE Error": "SectionErrorQueues",
            "OCR Error": "SectionErrorQueues",
            "KFE Error": "SectionErrorQueues",
            "MT Error": "SectionErrorQueues",
            "Media Conversion Error": "SectionErrorQueues",
            "SCAP Error": "SectionErrorQueues",
            "Missing Page Count": "NMECQueues",
            "Screening Team": "NMECQueues",
            "QC Team": "NMECQueues",
            "QA": "NMECQueues",
            "PreScreening": "NMECQueues",
            "QA Batch Checkout Error": "QAQueues",
            "QA Batch Name Mismatch": "QAQueues",
            "QA Batch Upgraded Classification": "QAQueues",
            "QA Batch define LOV Mappings": "QAQueues",
            "QA Perform Batch Merge": "QAQueues",
            "QA Reused Batch Name": "QAQueues",
            "QA Auto Update Failed": "QAQueues",
            "QA Checkout Error": "QAQueues",
            "QA Define LOV Mappings": "QAQueues",
            "QA Delete Requested": "QAQueues",
            "QA Downgrade Classification": "QAQueues",
            "QA Harmony Number Mismatch": "QAQueues",
            "QA Perform Merge": "QAQueues",
            "QA Upgraded Classification": "QAQueues",
            "QA Batch Missing Required Fields": "QAQueues",
            "QA Review Batch": "QAQueues",
            "Missing Required Fields": "QAQueues",
            "QA Duplicate Harmony Number": "QAQueues",
            "QA Invalid Classifications": "QAQueues",
            "QA Review": "QAQueues"
        }
    };
});