"""
Django settings for mypage project.

Generated by 'django-admin startproject' using Django 3.0.8.

For more information on this file, see
https://docs.djangoproject.com/en/3.0/topics/settings/

For the full list of settings and their values, see
https://docs.djangoproject.com/en/3.0/ref/settings/
"""

import os

# Build paths inside the project like this: os.path.join(BASE_DIR, ...)
BASE_DIR = os.path.dirname(os.path.dirname(os.path.abspath(__file__)))
TEMPLATE_PATH = os.path.join(BASE_DIR, "templates")
STATICFILES_DIRS = [
   os.path.join(BASE_DIR, "static"),
]

# Quick-start development settings - unsuitable for production
# See https://docs.djangoproject.com/en/3.0/howto/deployment/checklist/

# SECURITY WARNING: keep the secret key used in production secret!
SECRET_KEY = 'r&8*5!%p-puvleu5h&#!jm=^1s5(8=8(xk)n#1p+gdf+p@&bq5'

# SECURITY WARNING: don't run with debug turned on in production!
DEBUG = True

ALLOWED_HOSTS = ['*']


# Application definition

INSTALLED_APPS = [
    'website',
    'django.contrib.admin',
    'django.contrib.auth',
    'django.contrib.contenttypes',
    'django.contrib.sessions',
    'django.contrib.messages',
    'django.contrib.staticfiles',
    'rest_framework',
    
]

MIDDLEWARE = [
    'django.middleware.security.SecurityMiddleware',
    'django.contrib.sessions.middleware.SessionMiddleware',
    'django.middleware.common.CommonMiddleware',
    'django.middleware.csrf.CsrfViewMiddleware',
    'django.contrib.auth.middleware.AuthenticationMiddleware',
    'django.contrib.messages.middleware.MessageMiddleware',
    'django.middleware.clickjacking.XFrameOptionsMiddleware',
]

ROOT_URLCONF = 'mypage.urls'

TEMPLATES = [
    {
        'BACKEND': 'django.template.backends.django.DjangoTemplates',
        'DIRS': [
            # BASE_DIR / "challenges" / "templates" 
            # BASE_DIR / 'templates'
            os.path.join(BASE_DIR, 'templates')
            ],
        'APP_DIRS': True,
        'OPTIONS': {
            'context_processors': [
                'django.template.context_processors.debug',
                'django.template.context_processors.request',
                'django.contrib.auth.context_processors.auth',
                'django.contrib.messages.context_processors.messages',
            ],
        },
    },
]

WSGI_APPLICATION = 'mypage.wsgi.application'

ASGI_APPLICATION = 'mypage.asgi.application'


# Database
# https://docs.djangoproject.com/en/3.0/ref/settings/#databases

# DATABASES = {
#     'default': {
#         'ENGINE': 'django.db.backends.sqlite3',
#         'NAME': os.path.join(BASE_DIR, 'db.sqlite3'),
#     }
# }

#nhatvi
# DATABASES = {
#     'default': {
#         'ENGINE': 'djongo',
#         'NAME':  'trashweb',
#         'CLIENT':{
#             'host':'mongodb+srv://trashweb:<password>@trashweb.zhlaq1k.mongodb.net/?retryWrites=true&w=majority',
#             'username': 'trashweb',
#             'password': '0919562182qQ',
#             'authSource': 'admin',
#             'authMechanism': 'SCRAM-SHA-1',
#         },
#         'ENFORCE_SCHEMA': False,
#         'OBJECTID_ENABLED': True,
#     }
# }

#MongoDB Atlas
#vitrannhat8@gmail.com
#0919562182qQ
# DATABASES = {
#     'default': {
#         'ENGINE': 'djongo',
#         'NAME':  'trashweb',
#         'CLIENT':{
#             'host':'mongodb+srv://trashweb:<password>@trashweb.zrozfuo.mongodb.net/?retryWrites=true&w=majority',
#             'username': 'trashweb',
#             'password': '0919562182qQ',
#             'authSource': 'admin',
#             'authMechanism': 'SCRAM-SHA-1',
#         },
#         'ENFORCE_SCHEMA': False,
#         'OBJECTID_ENABLED': True,
#     }
# }

#PostgreSQL
DATABASES = {
    'default': {
        'ENGINE': 'django.db.backends.postgresql',
        'NAME': 'trashweb',
        'USER': 'postgres',
        'PASSWORD': '0919562182qQ',
        'HOST': 'localhost',
        'PORT': '5432',
    }
}

# Password validation
# https://docs.djangoproject.com/en/3.0/ref/settings/#auth-password-validators

AUTH_PASSWORD_VALIDATORS = [
    {
        'NAME': 'django.contrib.auth.password_validation.UserAttributeSimilarityValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.MinimumLengthValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.CommonPasswordValidator',
    },
    {
        'NAME': 'django.contrib.auth.password_validation.NumericPasswordValidator',
    },
]


# Internationalization
# https://docs.djangoproject.com/en/3.0/topics/i18n/

LANGUAGE_CODE = 'en-us'

TIME_ZONE = 'UTC'

USE_I18N = True

USE_L10N = True

USE_TZ = True


# Static files (CSS, JavaScript, Images)
# https://docs.djangoproject.com/en/3.0/howto/static-files/

STATIC_URL = '/static/'

MEDIA_ROOT = os.path.join(BASE_DIR, 'media')
MEDIA_URL = '/media/'

CRISPY_TEMPLATE_PACK = 'bootstrap4'

LOGIN_REDIRECT_URL = 'index'
LOGOUT_REDITECT_URL = 'login'
LOGIN_URL = 'login'

EMAIL_BACKEND = 'django.core.mail.backends.smtp.EmailBackend'
EMAIL_HOST = 'smtp.gmail.com'
EMAIL_PORT = 587
EMAIL_USE_TLS = True
EMAIL_PASS = '0919562182qQ'
EMAIL_USER = 'vitrannhat1@gmail.com'
EMAIL_HOST_USER = EMAIL_USER
EMAIL_HOST_PASSWORD = EMAIL_PASS

# EMAIL_HOST_USER = os.environ.get('EMAIL_USER')
# EMAIL_HOST_PASSWORD = os.environ.get('EMAIL_PASS')

DEFAULT_AUTO_FIELD = 'django.db.models.BigAutoField'

# CHANNEL_LAYERS = {
#     "default": {
#         "BACKEND": "channels_redis.core.RedisChannelLayer",
#         "CONFIG": {
#             "hosts": [("127.0.0.1", 6379)],
#         },
#     },
# }

