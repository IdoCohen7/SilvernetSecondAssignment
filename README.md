דרישות שמולאו:
1. יישום JSONAPI בפרויקט
2. התאמת הסוואגר לקבלת דאטא מהסוג המצופה בJSON:API - application/vnd.api+json.
3. יצירת משאבים עבור User ו-Tenant עם Controllers, Definitions ו-Services לכל אחד המספקים את מתודות ה-CRUD.
4. שימוש בולידציות, Try-Catch ו-Logging עם Serilog שמייצא את הלוגים לקובץ טקסט תחת תיקיית logs.
5. שינוי הראוטינג לפי הדרישה והטמעת ה-Relationship של User ו-Tenant שנאכף גם בלוגיקה ובשליחת הדאטא לשרת.
6. דריסת OnApplyFilter ב-UserResourceDefinition.
7. פיצול ל-WriteContext ו-ReadContext אשר מטפלים כל אחד בבקשות המתאימות (נבדק באמצעות לוגינג).
8. בקרת גישה עם JWT (שימוש ב-Hardcoded Credentials: ido@example.com, 123456) - ניתן לבדיקה בסוואגר.

הערה:
במהלך המימוש זיהיתי כי הפרדה מלאה בין Domain Entities לבין Resources של JsonApiDotNetCore, ללא שימוש בשכבת Repository (כנדרש), מחייבת תבנית ארכיטקטונית מסוימת שאינה מתוארת באופן מפורש בתיעוד הרשמי של הספרייה.
לאור מגבלת הזמן, בחרתי להתמקד במימוש יציב, תקני ומלא של JSON:API (Resources, Definitions, Services, Routing, Filtering, Logging ו־Access Control), תוך שמירה על מבנה קוד שניתן להרחבה והעמקה עתידית במידת הצורך.
אשמח כמובן להעמיק בנושא וללמוד אותו בהמשך!

תודה רבה!
