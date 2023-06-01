## **Project Description:**

Libro is a comprehensive Book Management System designed to facilitate the easy management and discovery of books in a library setting. The primary focus of this project is to design and implement the web APIs that will support the functionality of this application. These APIs will handle user registration and authentication, book transactions, patron profile management, book and author management, and more.

## **Main Features:**

### **User Registration and Authentication:**

Users should be able to register and log in to the system, and there should be different types of users (patrons, librarians, administrators) with different access levels.

**User Stories:**

- As a new user, I want to register an account so I can access the system.
- As a returning user, I want to log in to the system so I can access my account.
- As an administrator, I want to assign roles (patrons, librarians, administrators) to users so that users have appropriate access levels.

### **Searching and Browsing Books:**

Users should be able to search for books by title, author, genre, and other relevant criteria, and browse all available books. Each book's information page should include details such as its title, author, publication date, genre, and availability status, **and the result should be paginated**

**User Stories:**

- As a patron, I want to search for books by title, author, or genre so I can find a specific book.
- As a patron, I want to browse all available books so I can discover new books to read.
- As a patron, I want to see details of a book including its title, author, publication date, genre, and availability status when I select it, so I can decide if I want to borrow it.

### **Book Transactions:**

Patrons should be able to reserve available books. Librarians should be able to check out books to patrons and accept returned books. The system should keep track of due dates for borrowed books and whether a book is overdue.

**User Stories:**

- As a patron, I want to reserve available books so that I can borrow them later.
- As a librarian, I want to check out books to patrons so they can borrow them.
- As a librarian, I want to accept returned books so they become available for other patrons.
- As a librarian, I want to track due dates for borrowed books and identify overdue books, so I can manage book returns effectively.

### **Patron Profiles:**

Patrons should be able to view their own profile, which includes their borrowing history and any current or overdue loans. Librarians and administrators should be able to view and edit patron profiles.

**User Stories:**

- As a patron, I want to view my own profile, which includes my borrowing history and any current or overdue loans, so I can manage my borrowing activity.
- As a librarian or administrator, I want to view and edit patron profiles, so I can manage patron information and borrowing activity.

### **Book and Author Management:**

Librarians and administrators should be able to add, edit, or remove books and authors in the system. Administrators should be able to manage librarian accounts.

**User Stories:**

- As a librarian, I want to add, edit, or remove books in the system, so the system's book information remains accurate and up-to-date.
- As a librarian, I want to add, edit, or remove authors in the system, so the system's author information remains accurate and up-to-date.
- As an administrator, I want to manage librarian accounts, so I can ensure appropriate access to the system.

## **Additional Features (Optional):**

### **Reading Lists:**

Patrons could create and manage custom reading lists.

**User Story:**

- As a patron, I want to create and manage custom reading lists, so I can keep track of books I want to read.

### **Book Reviews and Ratings:**

Patrons could rate and review books, which would be visible to other users.

**User Stories:**

- As a patron, I want to rate and review books, so I can share my opinions with other users.
- As a patron, I want to see reviews and ratings by other patrons, so I can make informed decisions about which books to read.

### **Notifications:**

The system could send notifications to patrons about due dates, reserved books, or other important events.

**User Stories:**

- As a patron, I want to receive notifications about due dates, reserved books, or other important events, so I can stay updated about my borrowing activity.
- As a librarian, I want to send notifications to patrons about due dates, reserved books, or other important events, to keep them informed and promote timely book returns.

### **Book Recommendations:**

The system could provide personalized book recommendations to patrons based on their borrowing history or favorite genres.

**User Story:**

- As a patron, I want to receive personalized book recommendations based on my borrowing history or favorite genres, so I can discover new books that I might like.

## **Technical Requirements and Project Management:**

     In addition to the functional requirements outlined above, we'll place a strong emphasis on the technical execution and project management of the Libro application. We will be evaluating the design and implementation of the APIs, with particular attention to clean code principles, use of appropriate design patterns, and efficient handling of data and resources. The system should include robust error handling and logging, as well as secure **JWT** authentication and appropriate management of user permissions.

     For project management, we'll be looking at how effectively you use a tool like Jira to track progress and manage tasks.
