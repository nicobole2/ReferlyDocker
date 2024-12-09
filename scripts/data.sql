INSERT INTO Job.Company (registrationDate, status, name, taxId)
VALUES
(GETDATE(), 'Active', 'Tech Innovators', 'TI123456789'),
(GETDATE(), 'Active', 'Green Energy Ltd', 'GE987654321'),
(GETDATE(), 'Active', 'Finance Solutions', 'FS123789456'),
(GETDATE(), 'Active', 'Healthcare Partners', 'HP456123789'),
(GETDATE(), 'Active', 'EduTech Co', 'ET321654987'),
(GETDATE(), 'Active', 'Logistics Pro', 'LP789456123'),
(GETDATE(), 'Active', 'Creative Designs', 'CD654321987'),
(GETDATE(), 'Active', 'Retail Connect', 'RC987321654'),
(GETDATE(), 'Active', 'Cyber Defense', 'CD321987654'),
(GETDATE(), 'Active', 'Food & Beverages Inc', 'FB123456987');

INSERT INTO Job.JobSearch (published, positionName, category, subcategory, referenceId, description, companyContactId, share, state, workMode, area, vacancies, city, level)
VALUES 
(GETDATE(), 'Software Engineer', 'IT', 'Development', 10010, 'Develop and maintain software applications.', 1, 20.50, 'Open', 'Remote', 'Technology', 2, 'New York', 'Mid-Level'),
(GETDATE(), 'Data Analyst', 'IT', 'Analytics', 10020, 'Analyze data and create reports.', 2, 15.75, 'Open', 'On-Site', 'Business', 1, 'Los Angeles', 'Entry-Level'),
(GETDATE(), 'Project Manager', 'Management', 'Operations', 10030, 'Manage projects and teams.', 3, 25.00, 'Closed', 'Hybrid', 'Management', 1, 'Chicago', 'Senior'),
(GETDATE(), 'UX Designer', 'Design', 'User Experience', 10040, 'Design intuitive user interfaces.', 4, 18.00, 'Open', 'Remote', 'Design', 1, 'San Francisco', 'Mid-Level'),
(GETDATE(), 'HR Specialist', 'Human Resources', 'Recruitment', 10050, 'Handle recruitment and employee onboarding.', 5, 14.50, 'Open', 'On-Site', 'HR', 1, 'Boston', 'Entry-Level'),
(GETDATE(), 'Marketing Manager', 'Marketing', 'Digital Marketing', 10060, 'Develop marketing strategies.', 6, 22.00, 'Open', 'Hybrid', 'Marketing', 1, 'Seattle', 'Senior'),
(GETDATE(), 'Financial Analyst', 'Finance', 'Investment', 10070, 'Analyze financial data.', 7, 19.25, 'Open', 'On-Site', 'Finance', 1, 'New York', 'Mid-Level'),
(GETDATE(), 'Product Owner', 'IT', 'Product Management', 10080, 'Define product vision and backlog.', 8, 24.00, 'Closed', 'Remote', 'Product', 1, 'Austin', 'Senior'),
(GETDATE(), 'Business Analyst', 'Business', 'Strategy', 10090, 'Analyze business processes.', 9, 17.00, 'Open', 'Hybrid', 'Business', 1, 'Denver', 'Mid-Level'),
(GETDATE(), 'Customer Support Agent', 'Customer Service', 'Support', 10100, 'Provide customer support.', 10, 12.50, 'Open', 'Remote', 'Support', 3, 'Miami', 'Entry-Level')


INSERT INTO Job.Requirements (jobReferenceId, description)
VALUES
(10010, 'Bachelor’s degree in Computer Science.'),
(10010, '3+ years of software development experience.'),
(10020, 'Proficiency in SQL and data visualization tools.'),
(10020, 'Strong analytical and problem-solving skills.'),
(10030, 'Proven experience in project management.'),
(10030, 'Excellent communication and leadership skills.'),
(10040, 'Experience with design tools like Figma or Sketch.'),
(10040, 'Understanding of UX principles and best practices.'),
(10050, 'Bachelor’s degree in Human Resources or related field.'),
(10050, 'Experience with recruitment software.')


INSERT INTO Job.Responsibilities (jobReferenceId, description)
VALUES
(10010, 'Develop and maintain software features.'),
(10010, 'Collaborate with cross-functional teams.'),
(10020, 'Analyze large datasets to generate insights.'),
(10020, 'Prepare data reports for stakeholders.'),
(10030, 'Plan and oversee project execution.'),
(10030, 'Communicate project status to leadership.'),
(10040, 'Design user interfaces based on user research.'),
(10040, 'Create wireframes and prototypes.'),
(10050, 'Source and recruit candidates for open roles.'),
(10050, 'Coordinate onboarding for new hires.')