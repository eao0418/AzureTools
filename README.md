# AzureTools
This is a collection of tools that will help someone who is looking for more information on their Azure environment
... or someone else's. 

## Project Overview
This project is the baseline for other toolkits that could make integrating Azure information with other security tools, for completing inventories, or for doing recon post-exploitation. 

## Current Problems
My testing environments do not support all of the properties from various endpoints, so as of this commit only user and group collection is working. 

## Future Improvements
- Groupmember enumeration, service principal, app registration, app owner enumeration, and directory roles collected. 
- CLI tools to get information about target tenants. 
- Database persistence mechanisms. 
- Automation tools.

## Requirements
- Run the kafka docker container locally and add the required topics. A really good tool is here: https://developer.confluent.io/confluent-tutorials/kafka-on-docker/